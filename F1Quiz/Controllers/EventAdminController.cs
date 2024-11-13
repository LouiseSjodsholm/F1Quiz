using F1Quiz.Data;
using F1Quiz.Models;
using F1Quiz.Models.ViewModels;
using F1Quiz.Repositories;
using F1Quiz.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace F1Quiz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventAdminController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ScoreCalculation _scoreCalculation;

        public EventAdminController(IQuestionRepository questionRepository, IEventRepository eventRepository, ScoreCalculation scoreCalculation)
        {
            _questionRepository = questionRepository;
            _eventRepository = eventRepository;
            _scoreCalculation = scoreCalculation;
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            var model = new EventWithQuestionsViewModel
            {
                Questions = Enumerable.Range(0, 7).Select(_ => new QuestionViewModel()).ToList() // Initialize with 7 empty questions
            };
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventWithQuestionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Re-initialize with 7 questions if validation fails, as these would otherwise be lost
                model.Questions = Enumerable.Range(0, 7).Select(_ => new QuestionViewModel()).ToList();
                return View(model);
            }

            // image
            string imagePath = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0) 
            {
                //directory within wwwroot to store image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(uploadsFolder);

                //unique name --> no overwrite of file
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //save
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(filestream);
                }
                imagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/"); //relative path
            }
            
            //create event instance
            var newEvent = new Event
            {
                RaceName = model.Name,
                RaceDateTime = model.DateTime,
                Description = model.Description,
                ImagePath = imagePath,
                Questions = model.Questions
                                 .Where(q => !string.IsNullOrWhiteSpace(q.Text)) // Only save questions with text
                                 .Select(q => new Question 
                                 { 
                                     QuestionText = q.Text, 
                                     AnswerType = q.AnswerType, 
                                     Options = q.AnswerType == "mcq" ? q.Options?.Split(',').Select(o => o.Trim()).ToList() : null,
                                     DriverOptions = q.AnswerType == "allDriver" ? PredefinedOptions.AllDrivers : null
                                 }).ToList()
            };

            //save
            bool isSaved = await _eventRepository.AddEventAsync(newEvent);
            if (isSaved)
                ViewData["SuccessMessage"] = "Race added, it is now open for responses.";
            else
                ViewData["ErrorMessage"] = "There was an issue with saving your race event, please try again.";
            return View();
        }

        public async Task<IActionResult> ListEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return View(events);
        }

        
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            var race = await _eventRepository.GetEventByIdAsync(id);
            if (race == null)
            {
                ViewData["ErrorMessage"] = "Race not found";
                return RedirectToAction("ListEvents");
            }

            // Map the event data to EventWithQuestionsViewModel
            var model = new EventWithQuestionsViewModel
            {
                EventId = race.Id,
                Name = race.RaceName,
                DateTime = race.RaceDateTime,
                Description = race.Description,
                ImagePath = race.ImagePath,
                Questions = race.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.Id,
                    Text = q.QuestionText,
                    AnswerType = q.AnswerType,
                    Options = q.AnswerType == "mcq" ? string.Join(", ", q.Options) : null,
                    DriverOptions = q.AnswerType == "allDriver" ? q.DriverOptions : null,
                    CorrectAnswer = q.CorrectAnswer // Populate existing correct answer
                }).ToList()
            };

            // Ensure there are always 7 questions for editing
            if (model.Questions.Count < 7)
            {
                model.Questions.AddRange(Enumerable.Range(0, 7 - model.Questions.Count)
                    .Select(_ => new QuestionViewModel()));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(EventWithQuestionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Re-fetch the existing data to repopulate non-editable fields
                var existingEvent = await _eventRepository.GetEventByIdAsync((int)model.EventId);
                if (existingEvent != null)
                {
                    model.Name = existingEvent.RaceName;
                    model.DateTime = existingEvent.RaceDateTime;
                    model.Description = existingEvent.Description;
                    model.ImagePath = existingEvent.ImagePath;
                    model.Questions = existingEvent.Questions.Select(q => new QuestionViewModel
                    {
                        QuestionId = q.Id,
                        Text = q.QuestionText,
                        AnswerType = q.AnswerType,
                        Options = q.AnswerType == "mcq" ? string.Join(", ", q.Options) : null,
                        DriverOptions = q.AnswerType == "allDriver" ? q.DriverOptions : null,
                        CorrectAnswer = model.Questions.FirstOrDefault(mq => mq.QuestionId == q.Id)?.CorrectAnswer
                    }).ToList();
                }

                // Return the view with the populated model
                return View(model);
            }

            var existingEventToUpdate = await _eventRepository.GetEventByIdAsync((int)model.EventId);
            if (existingEventToUpdate == null)
            {
                ViewData["ErrorMessage"] = "Race not found";
                return RedirectToAction("ListEvents");
            }

            // Update only the CorrectAnswer for each question
            foreach (var modelQuestion in model.Questions)
            {
                var existingQuestion = existingEventToUpdate.Questions
                    .FirstOrDefault(q => q.Id == modelQuestion.QuestionId);

                if (existingQuestion != null)
                    existingQuestion.CorrectAnswer = modelQuestion.CorrectAnswer;
            }

            // Save changes
            bool isUpdated = await _eventRepository.UpdateEventAsync(existingEventToUpdate);
            if (isUpdated)
            {
                ViewData["SuccessMessage"] = "Correct answers updated successfully.";
                // Automatically calculate and save scores for the event
                try
                {
                    await _scoreCalculation.CalculateAndSaveScoresAsync((int)model.EventId);
                    TempData["SuccessMessage"] += " Scores calculated and saved.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error calculating scores: {ex.Message}";
                }
            }  
            else
                ViewData["ErrorMessage"] = "There was an issue with updating the correct answers, please try again.";

            return RedirectToAction("ListEvents");
        }
    }
}

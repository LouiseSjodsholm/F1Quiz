using F1Quiz.Data;
using F1Quiz.Models;
using F1Quiz.Models.ViewModels;
using F1Quiz.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection;

namespace F1Quiz.Controllers
{
    public class EventController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IResponseRepository _responseRepository;

        public EventController(IQuestionRepository questionRepository, IEventRepository eventRepository, IResponseRepository responseRepository)
        {
            _questionRepository = questionRepository;
            _eventRepository = eventRepository;
            _responseRepository = responseRepository;
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

        [HttpGet]
        public async Task<IActionResult> AnswerQuestions()
        {
            //Get event in future closes to now
            var upcomingEvent = await _eventRepository.GetUpcomingEventAsync(DateTime.Now);
            if(upcomingEvent == null)
            {
                ViewData["ErrorMessage"] = "There's currently no upcoming event. Please check back again later";
                return View();
            }

            var viewModel = new AnswerQuestionsViewModel
            {
                EventId = upcomingEvent.Id,
                EventName = upcomingEvent.RaceName,
                Description = upcomingEvent.Description,
                RaceDateTime = upcomingEvent.RaceDateTime,
                ImagePath = upcomingEvent.ImagePath,
                Questions = upcomingEvent.Questions.Select(q => new QuestionResponseViewModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    AnswerType = q.AnswerType,
                    Options = q.AnswerType == "mcq" ? q.Options : null,
                    DriverOptions = q.AnswerType=="allDriver" ? PredefinedOptions.AllDrivers : null
                }).ToList()
            };

            return View("AnswerQuestions", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerQuestions(AnswerQuestionsViewModel responses)
        {
            var raceRespondedTo = await _eventRepository.GetEventByIdAsync(responses.EventId);
            if (raceRespondedTo == null || raceRespondedTo.RaceDateTime <= DateTime.Now)
            {
                ViewData["ErrorMessage"] = "The event has ended.";
                return View();
            }
                

            var responseList = responses.Questions.Select(r=> new Response
            {
                QuestionId = r.QuestionId,
                Answer = r.Response //Remember to add which user
            }).ToList();
            bool isSaved = await _responseRepository.AddResponseAsync(responseList);
            if (isSaved)
                ViewData["SuccessMessage"] = "Tak for dit svar!";
            else
                ViewData["ErrorMessage"] = "Der var et problem med at gemme dit svar, prøv igen.";
            return View();
        }
    }
}

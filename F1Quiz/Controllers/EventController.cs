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

        public EventController(IQuestionRepository questionRepository, IEventRepository eventRepository)
        {
            _questionRepository = questionRepository;
            _eventRepository = eventRepository;
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
            
            var newEvent = new Event
            {
                RaceName = model.Name,
                RaceDateTime = model.DateTime,
                Description = model.Description,
                ImagePath = imagePath,
                Questions = model.Questions
                                 .Where(q => !string.IsNullOrWhiteSpace(q.Text)) // Only save questions with text
                                 .Select(q => new Question { QuestionText = q.Text, AnswerType = q.AnswerType })
                                 .ToList()
            };

            await _eventRepository.AddEventAsync(newEvent);
            return RedirectToAction("Index", "Home");
        }

    }
}

using F1Quiz.Data;
using F1Quiz.Models;
using F1Quiz.Models.ViewModels;
using F1Quiz.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1Quiz.Controllers
{
    public class QuizController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IResponseRepository _responseRepository;
        public QuizController(IEventRepository eventRepository, IResponseRepository responseRepository)
        {
            _eventRepository = eventRepository;
            _responseRepository = responseRepository;
        }
        [HttpGet]
        public async Task<IActionResult> AnswerQuestions()
        {
            //Get event in future closes to now
            var upcomingEvent = await _eventRepository.GetUpcomingEventAsync(DateTime.Now);
            if (upcomingEvent == null)
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
                    DriverOptions = q.AnswerType == "allDriver" ? PredefinedOptions.AllDrivers : null
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


            var responseList = responses.Questions.Select(r => new Response
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

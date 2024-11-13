using F1Quiz.Models;
using F1Quiz.Repositories;

namespace F1Quiz.Services
{
    public class ScoreCalculation
    {
        private readonly IEventRepository _eventRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IScoreRepository _scoreRepository;

        public ScoreCalculation(
            IEventRepository eventRepository,
            IResponseRepository responseRepository,
            IScoreRepository scoreRepository)
        {
            _eventRepository = eventRepository;
            _responseRepository = responseRepository;
            _scoreRepository = scoreRepository;
        }

        public async Task CalculateAndSaveScoresAsync(int eventId)
        {
            var eventDetails = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventDetails == null || eventDetails.Questions.All(q => q.CorrectAnswer == null))
            {
                throw new InvalidOperationException("Event or correct answers not found.");
            }

            var responses = await _responseRepository.GetResponsesByEventIdAsync(eventId);
            var userResponses = responses.GroupBy(r => r.UserId);

            foreach (var userResponseGroup in userResponses)
            {
                int correctAnswersCount = 0;
                var userId = userResponseGroup.Key;

                foreach (var response in userResponseGroup)
                {
                    var correctAnswer = eventDetails.Questions
                        .FirstOrDefault(q => q.Id == response.QuestionId)?.CorrectAnswer;

                    if (correctAnswer != null && response.Answer == correctAnswer)
                    {
                        correctAnswersCount++;
                    }
                }

                var score = new Score
                {
                    UserId = userId.ToString(),
                    EventId = eventId,
                    Points = correctAnswersCount
                };

                await _scoreRepository.AddScoreAsync(score);
            }
        }
    }
}

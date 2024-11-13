using F1Quiz.Data;
using F1Quiz.Models;
using F1Quiz.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1Quiz.Repositories
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly QuizContext _context;
        private readonly IEventRepository _eventRepository;
        public ScoreRepository(QuizContext context, IEventRepository eventRepository) 
        {
            _context = context; 
            _eventRepository = eventRepository;
        }
        public async Task<bool> AddScoreAsync(Score score)
        {
            _context.Scores.Add(score);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0;
        }
        public async Task GetEventLeaderboardAsync(int eventId)
        {
            var scores = await _context.Scores
                .Where(s => s.EventId == eventId)
                .Select(s => new UserScoreViewModel
                {
                    UserName = s.User.UserName,
                    Score = s.Points
                })
                .OrderByDescending(s => s.Score)
                .ToListAsync();

            if (!scores.Any())
            {
                throw new InvalidOperationException("Event not found or no scores available for this event");
            }
        }

        public async Task<EventLeaderboardViewModel> GetLatestEventLeaderboardAsync()
        {
            var latestEventWithScores = await _context.Scores
                .Include(s => s.Event)
                .ThenInclude(e => e.Questions)
                .Where(s => s.Event.Questions.All(q => q.CorrectAnswer != null))
                .OrderByDescending(s => s.Event.RaceDateTime)
                .Select(s => s.Event)
                .FirstOrDefaultAsync();

            if (latestEventWithScores == null)
            {
                throw new InvalidOperationException("No events with calculated scores found.");
            }

            // Retrieve scores for this event
            var scores = await _context.Scores
                .Where(s => s.EventId == latestEventWithScores.Id)
                .Select(s => new UserScoreViewModel
                {
                    UserName = s.User.UserName,
                    Score = s.Points
                })
                .OrderByDescending(s => s.Score)
                .ToListAsync();

            return new EventLeaderboardViewModel
            {
                RaceName = latestEventWithScores.RaceName,
                RaceDate = latestEventWithScores.RaceDateTime,
                UserScores = scores
            };
        }

        public async Task<TotalLeaderboardViewModel> GetTotalLeaderboardAsync()
        {
            var totalScores = await _context.Scores
                .GroupBy(s => s.UserId)
                .Select(g => new UserTotalScoreViewModel
                {
                    UserName = g.First().User.UserName,
                    TotalScore = g.Sum(s => s.Points)
                })
                .OrderByDescending(u => u.TotalScore)
                .ToListAsync();

            return new TotalLeaderboardViewModel
            {
                UserScores = totalScores
            };
        }
    }
}

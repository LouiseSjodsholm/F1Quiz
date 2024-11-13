using F1Quiz.Models;
using F1Quiz.Models.ViewModels;

namespace F1Quiz.Repositories
{
    public interface IScoreRepository
    {
        Task<bool> AddScoreAsync(Score score);
        Task GetEventLeaderboardAsync(int eventId);
        Task<EventLeaderboardViewModel> GetLatestEventLeaderboardAsync();
        Task<TotalLeaderboardViewModel> GetTotalLeaderboardAsync();
    }
}

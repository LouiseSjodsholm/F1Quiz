namespace F1Quiz.Models.ViewModels
{
    public class EventLeaderboardViewModel
    {
        public string RaceName { get; set; }
        public DateTime RaceDate { get; set; }
        public List<UserScoreViewModel> UserScores { get; set; } = new List<UserScoreViewModel>();
    }

    public class UserScoreViewModel
    {
        public string UserName { get; set; }
        public int Score { get; set; }
    }
}

namespace F1Quiz.Models.ViewModels
{
    public class TotalLeaderboardViewModel
    {
        public List<UserTotalScoreViewModel> UserScores { get; set; } = new List<UserTotalScoreViewModel>();
    }
    public class UserTotalScoreViewModel
    {
        public string UserName { get; set; }
        public int TotalScore { get; set; }
    }
}

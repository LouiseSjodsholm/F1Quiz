using System.ComponentModel.DataAnnotations;

namespace F1Quiz.Models.ViewModels
{
    public class AnswerQuestionsViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string? Description { get; set; }
        public DateTime RaceDateTime { get; set; }
        public string? ImagePath { get; set; }
        public List<QuestionResponseViewModel> Questions { get; set; } = new List<QuestionResponseViewModel>();
    }

    public class QuestionResponseViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerType { get; set; } //Determines what type of anwser input should be displayed 

        [Required(ErrorMessage = "Please answer question")]
        public string Response { get; set; } //User input
        public List<string>? Options { get; set; } //Options for multiple choice questions
    }
}

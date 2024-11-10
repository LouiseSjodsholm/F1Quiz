using System.ComponentModel.DataAnnotations;

namespace F1Quiz.Models.ViewModels
{
    public class EventWithQuestionsViewModel
    {
        [Required(ErrorMessage ="Race name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Event date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        public string? Description { get; set; }

        public IFormFile? ImageFile { get; set; } //To add photo for each event
        public string? ImagePath { get; set; }

        [Required(ErrorMessage ="Questions are required.")]
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

    public class QuestionViewModel
    {
        [Required(ErrorMessage ="Question text is required.")]
        public string Text { get; set; }
        public string AnswerType { get; set; } = "text"; //defaults to text unless otherwise specified during creation.
        public string? Options { get; set; } //Multiple choice options
    }
}

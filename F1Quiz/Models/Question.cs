namespace F1Quiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public required string QuestionText { get; set; }
        public string? CorrectAnswer { get; set; }
        public string AnswerType { get; set; } = "text"; //default to text unless otherwise specified when created
        public List<string>? Options { get; set; } //Used for multiple choice

        //Foreign key to which event the question belongs to
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}

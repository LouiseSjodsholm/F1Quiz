namespace F1Quiz.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int QuestionId { get; set; }
        public required string Answer {  get; set; }
        public bool? IsCorrect { get; set; }
    }
}

namespace F1Quiz.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        
        public required string Answer {  get; set; }
        public int UserId { get; set; }
    }
}

namespace F1Quiz.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int Points { get; set; }
    }
}

namespace F1Quiz.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime RaceDateTime { get; set; }
        public string RaceName { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; } //Questions associated with this event
        
        public string? ImagePath { get; set; } //save path to event image

        public Event() 
        {
            Questions = new List<Question>();
        }
    }
}

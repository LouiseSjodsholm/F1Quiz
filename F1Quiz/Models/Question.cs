using F1Quiz.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace F1Quiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public required string QuestionText { get; set; }
        public string? CorrectAnswer { get; set; }
        public string AnswerType { get; set; } = "text"; //default to text unless otherwise specified when created
        public List<string>? Options { get; set; } //Used for multiple choice
        public string? DriverOptionsJson { get; set; }

        [NotMapped]
        public List<DriverOption>? DriverOptions
        {
            get => DriverOptionsJson == null ? null :
                JsonSerializer.Deserialize<List<DriverOption>>(DriverOptionsJson);
            set => DriverOptionsJson = value == null ? null :
                JsonSerializer.Serialize(value);
        }

        //Foreign key to which event the question belongs to
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}

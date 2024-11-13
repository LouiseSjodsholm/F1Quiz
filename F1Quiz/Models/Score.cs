using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1Quiz.Models
{
    public class Score
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int Points { get; set; }
    }
}

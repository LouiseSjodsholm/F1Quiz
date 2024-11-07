using Microsoft.EntityFrameworkCore;
using F1Quiz.Models;

namespace F1Quiz.Data
{
    public class QuizContext:DbContext
    {
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Score> Scores { get; set; }

        public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }
    }
}

using Microsoft.EntityFrameworkCore;
using F1Quiz.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace F1Quiz.Data
{
    public class QuizContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Score> Scores { get; set; }

        public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>()
                .Property(q => q.DriverOptionsJson)
                .HasColumnType("nvarchar(max)");

            //Event - Question cascade delete
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Event)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            //Question - Response restricted delete, so no error occurs due to multiple cascading delete from event->question->response
            modelBuilder.Entity<Response>()
                .HasOne(r=>r.Question)
                .WithMany()
                .HasForeignKey(r=>r.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

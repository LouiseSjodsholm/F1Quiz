using F1Quiz.Data;
using F1Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace F1Quiz.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly QuizContext _context;

        public EventRepository(QuizContext context)
        {
            _context = context; //DbContext
        }
        public async Task<bool> AddEventAsync(Event race)
        {
            _context.Events.Add(race);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0;
        }

        public async Task DeleteEventAsync(int id)
        {
            //Delete responses associated with questions for this event
            var questions = _context.Questions.Where(q=>q.EventId==id).ToList();
            foreach (var question in questions)
            {
                var responses = _context.Responses.Where(r => r.QuestionId == question.Id);
                _context.Responses.RemoveRange(responses);
            }
            _context.Questions.RemoveRange(questions);

            //Delete event
            var race = await _context.Events.FindAsync(id);
            if (race != null) 
            {
                _context.Events.Remove(race);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id); //return null if no matches
        }

        public async Task<Event?> GetUpcomingEventAsync(DateTime currentDateTime)
        {
            return await _context.Events
                .Where(e=>e.RaceDateTime > currentDateTime)
                .OrderBy(e=>e.RaceDateTime)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateEventAsync(Event race)
        {
            _context.Entry(race).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

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
        public async Task AddEventAsync(Event race)
        {
            _context.Events.Add(race);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
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

        public async Task UpdateEventAsync(Event race)
        {
            _context.Entry(race).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

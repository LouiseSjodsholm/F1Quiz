using F1Quiz.Models;

namespace F1Quiz.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task AddEventAsync(Event race);
        Task UpdateEventAsync(Event race);
        Task DeleteEventAsync(int id);
    }
}

using F1Quiz.Models;

namespace F1Quiz.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetUpcomingEventAsync(DateTime currentDateTime);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task<bool> AddEventAsync(Event race);
        Task UpdateEventAsync(Event race);
        Task DeleteEventAsync(int id);
    }
}

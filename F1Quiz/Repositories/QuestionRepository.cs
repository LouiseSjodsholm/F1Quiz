using F1Quiz.Data;
using F1Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace F1Quiz.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizContext _context;

        public QuestionRepository(QuizContext context)
        {
            _context = context; //DbContext
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.FindAsync(id); //Return null if theres no question with the id
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsByEventAsync(int eventId)
        {
            return await _context.Questions.Where(q =>  q.EventId == eventId).ToListAsync();
        }

        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}

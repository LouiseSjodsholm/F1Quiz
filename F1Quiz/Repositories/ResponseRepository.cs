﻿using F1Quiz.Data;
using F1Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace F1Quiz.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly QuizContext _context;
        public ResponseRepository(QuizContext context) 
        {
            _context = context;
        }
        public async Task<bool> AddResponseAsync(List<Response> responses)
        {
            _context.Responses.AddRange(responses);
            int addedRows = await _context.SaveChangesAsync();
            return addedRows > 0;
        }

        public async Task<List<Response>> GetAllResponsesAsync()
        {
            return await _context.Responses.ToListAsync();
        }

        public async Task<List<Response>> GetResponsesByEventIdAsync(int eventId)
        {
            return await _context.Responses.Where(r => r.Question.EventId == eventId).ToListAsync();
        }
    }
}

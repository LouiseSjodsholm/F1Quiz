﻿using F1Quiz.Models;

namespace F1Quiz.Repositories
{
    public interface IResponseRepository
    {
        Task AddResponseAsync(List<Response> responses);
        Task<List<Response>> GetResponsesByEventIdAsync(int eventId);
        Task<List<Response>> GetAllResponsesAsync();
    }
}
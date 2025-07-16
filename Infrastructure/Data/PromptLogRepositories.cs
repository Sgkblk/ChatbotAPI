using Chatbot.Application.Repositories;
using Chatbot.Domain.Entities;
using Chatbot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class PromptLogRepository : IPromptLogRepository
{
    private readonly PromptDbContext _context;

    public PromptLogRepository(PromptDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PromptLog>> GetAllAsync()
    {
        return await _context.PromptLogs.ToListAsync();
    }

    public async Task SavePromptAsync(PromptLog log)
    {
        _context.PromptLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}

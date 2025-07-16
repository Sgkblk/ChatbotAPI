namespace Chatbot.Infrastructure.Services
{
    public interface IAIService
    {
        Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!);
    }
}

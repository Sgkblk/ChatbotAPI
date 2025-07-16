using Chatbot.Domain.Entities;

namespace Chatbot.Application.Repositories
{
    public interface IPromptLogRepository
    {
      Task SavePromptAsync(PromptLog log);
        //detaylar uygulama katmanında gizlenir.
    }
}

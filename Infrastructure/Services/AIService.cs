using Chatbot.Domain.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Chatbot.Infrastructure.Services
{
    public class AIService(IHubContext<AIHub> hubContext, IChatCompletionService chatCompletionService, Kernel kernel) : IAIService
    {
        //LLM çağrısı için kullanılan servis..
        public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!)
        {
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            //o bağlantıya ait sohbet geçmişi...
            var history = HistoryService.GetChatHistory(connectionId);

            history.AddUserMessage(prompt);
            string responseContent = "";
            try
            {
                //cevapları al. (parçalı olarak)
                await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: kernel))
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    //parçalı cevapları birleştir..Gerçek zamanlı olarak client'a gönder..  
                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", response.ToString());
                    responseContent += response.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            history.AddAssistantMessage(responseContent);
        }
    }
}

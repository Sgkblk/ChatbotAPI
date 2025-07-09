namespace Chatbot.Services
{
    public interface IChatService
    {
        void SendMessage(string message);
        string ReceiveMessage();
    }
}

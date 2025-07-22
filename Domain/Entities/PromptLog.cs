using System;

namespace Chatbot.Domain.Entities
{
    public class PromptLog
    {
        //PromptLog sınıfı, kullanıcıların girdikleri promptları ve AI tarafından verilen cevapları kaydetmek için
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Prompt { get; set; }
        public required string Response { get; set; }
        public DateTime PromptDate { get; set; }
        public DateTime ResponseDate { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
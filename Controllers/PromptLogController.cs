using Chatbot.Application.Repositories;
using Chatbot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Chatbot.Application.Services;

namespace Chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptLogController : ControllerBase
    {
        private readonly IPromptLogRepository _repository;

        private readonly IAIService _aiService;

        public PromptLogController(IPromptLogRepository repository, IAIService aiService)
        {
            _repository = repository;
            _aiService = aiService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPromptLog([FromBody] PromptLog log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. AI'dan cevap al
            var response = await _aiService.GetAIResponseAsync(log.Prompt);

            // 2. Cevabı log objesine yaz  
            log.Response = response;
            log.Timestamp = DateTime.UtcNow;

            // 3. Veritabanına kaydetmek için log objesini repository'e gönder
            await _repository.SavePromptAsync(log);

            // 4. Cevabı dön
            return Ok(new { response });
        }

    }
}

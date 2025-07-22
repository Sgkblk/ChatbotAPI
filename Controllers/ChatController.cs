using Chatbot.Domain.ViewModels;
using Chatbot.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IAIService _aiService;

        public ChatController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost]
        public async Task<IActionResult> PostChatAsync([FromBody] ChatRequestVM chatRequest, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(chatRequest.Prompt) || string.IsNullOrWhiteSpace(chatRequest.ConnectionId))
            {
                return BadRequest("Prompt and ConnectionId are required.");
            }

            await _aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken);
            return Ok();
        }
    }
}

using Chatbot.Application.Repositories;
using Chatbot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Chatbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromptLogController : ControllerBase
    {
        private readonly IPromptLogRepository _repository;

        public PromptLogController(IPromptLogRepository repository)
        {
            _repository = repository;
        }

        [HttpPost] 
        public async Task<IActionResult> PostPromptLog([FromBody] PromptLog log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.SavePromptAsync(log);
            return Ok("Log başarıyla kaydedildi...");
        }
    }
}

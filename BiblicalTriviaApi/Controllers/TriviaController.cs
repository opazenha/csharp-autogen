using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BiblicalTriviaApi.Services;
using BiblicalTriviaApi.Models;

namespace BiblicalTriviaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TriviaController : ControllerBase
    {
        private readonly TriviaService _triviaService;

        public TriviaController(TriviaService triviaService)
        {
            _triviaService = triviaService;
        }

        [HttpGet("question")]
        public async Task<ActionResult<TriviaQuestion>> GetQuestion(
            [FromQuery] string? category = null,
            [FromQuery] string? difficulty = null)
        {
            try
            {
                var question = await _triviaService.GenerateQuestionAsync(category, difficulty);
                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

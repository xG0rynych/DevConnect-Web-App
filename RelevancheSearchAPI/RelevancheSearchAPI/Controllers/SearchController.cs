using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using OpenAI_API;
using RelevancheSearchAPI.Data;
using Microsoft.IdentityModel.Tokens;
using RelevancheSearchAPI.DTO;
namespace RelevancheSearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        Facade _facade;
        public SearchController(SearchDbContext context, IConfiguration configuration)
        {
            _facade = new Facade(context, configuration);
        }

        [HttpPost("articles")]
        public async Task<ActionResult> AddArticleAsync([FromBody] EntityDTO entity)
        {
            if (entity == null || entity.Id == default || entity.Title == null)
                return StatusCode(400);

            bool result = await _facade.AddArticleAsync(entity.Id, entity.Title);
            if (result == true)
                return Ok();
            else
                return StatusCode(404);
        }

        [HttpGet("articles/similar/{id}")]
        public async Task<ActionResult<List<int>?>> GetSimilarArticles(int id)
        {
            var result = await _facade.GetSimilarArticles(id);
            if (result.IsNullOrEmpty() == false)
                return Ok(result);
            else
                return StatusCode(404);
        }

        [HttpGet("articles/bytitle/{title}")]
        public async Task<ActionResult<List<int>?>> SearchArticles(string title)
        {
            var result = await _facade.SearchArticlesAsync(title);
            if (result.IsNullOrEmpty() == false)
                return Ok(result);
            else
                return StatusCode(404);
        }

        [HttpDelete("articles/{id}")]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            bool result = await _facade.RemoveArticle(id);
            if (result == true)
                return Ok();
            else
                return StatusCode(404);
        }



        [HttpPost("questions")]
        public async Task<ActionResult> AddQuestionAsync([FromBody] EntityDTO entity)
        {
            if (entity == null || entity.Id == default || entity.Title == null)
                return StatusCode(400);

            bool result = await _facade.AddQuestionAsync(entity.Id, entity.Title);
            if (result == true)
                return Ok();
            else
                return StatusCode(409);
        }

        [HttpGet("questions/similar/{id}")]
        public async Task<ActionResult<List<int>?>> GetSimilarQuestions(int id)
        {
            var result = await _facade.GetSimilarQuestions(id);
            if (result.IsNullOrEmpty() == false)
                return Ok(result);
            else
                return StatusCode(404);
        }

        [HttpGet("questions/bytitle/{title}")]
        public async Task<ActionResult<List<int>?>> SearchQuestions(string title)
        {
            var result = await _facade.SearchQuestionsAsync(title);
            if (result.IsNullOrEmpty() == false)
                return Ok(result);
            else
                return StatusCode(404);
        }

        [HttpDelete("questions/{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            bool result = await _facade.RemoveQuestion(id);
            if (result == true)
                return Ok();
            else
                return StatusCode(404);
        }
    }
}

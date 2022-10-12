using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/Articles
        [HttpGet]
        [Route("Articles")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetArticles(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetArticlesAsync();
            return Ok(articles);
        }

        // GET: api/Articles/MostCommentable
        [HttpGet]
        [Route("Articles/MostCommentable")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetMostCommentableArticles(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetMostCommentableArticlesAsync();
            return Ok(articles);
        }

        // GET: api/Article/MostLikeable
        [HttpGet]
        [Route("Articles/MostLikeable")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetMostLikeableArticles(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetMostLikeableArticlesAsync();
            return Ok(articles);
        }

        // GET: api/Article/BySubscription
        [HttpGet]
        [Route("Articles/BySubscription")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetArticlesBySubscription(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetArticlesBySubscriptionAsync();
            return Ok(articles);
        }

        // GET: api/Article/5
        [HttpGet]
        [Route("Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadFullArticleDto>> GetArticle([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleDto article = await _articleService.GetArticleAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // GET: api/FullArticle/5
        [HttpGet]
        [Route("FullArticle/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadFullArticleDto>> GetFullArticle([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadFullArticleDto article = await _articleService.GetFullArticleAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // GET: api/Article/5/Comments
        [HttpGet]
        [Route("Article/{id:Guid}/Comments")]
        [Authorize]
        public async Task<ActionResult<ReadArticleCommentsDto>> GetArticleComments([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleCommentsDto article = await _articleService.GetArticleCommentsAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // GET: api/Article/5/Likes
        [HttpGet]
        [Route("Article/{id:Guid}/Likes")]
        [Authorize]
        public async Task<ActionResult<ReadArticleLikesDto>> GetArticlelikes([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleLikesDto article = await _articleService.GetArticleLikesAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PATCH: api/Article/5
        [HttpPatch]
        [Route("Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadArticleDto>> PatchArticle([FromRoute] Guid id, JsonPatchDocument<Article> articleUpdates, CancellationToken token = default)
        {
            ReadArticleDto article = await _articleService.GetArticleAsync(id);
            if (article == null)
            {
                return BadRequest();
            }
            ReadArticleDto modifiedArticle = await _articleService.PatchArticleAsync(id, articleUpdates);
            return Ok(modifiedArticle);
        }

        // PUT: api/Article/5
        [HttpPut]
        [Route("Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadArticleDto>> PutArticle([FromRoute] Guid id, [FromBody] CreateArticleDto article, CancellationToken token = default)
        {
            ReadArticleDto articleToModify = await _articleService.GetArticleAsync(id);
            if (articleToModify == null)
            {
                return BadRequest();
            }

            ReadArticleDto newArticle = await _articleService.UpdateArticleAsync(id, article);
            return Ok(newArticle);   
        }

        // POST: api/Article
        [HttpPost]
        [Route("Article")]
        [Authorize]
        public async Task<ActionResult<ReadArticleDto>> PostArticle([FromBody]CreateArticleDto article, CancellationToken token = default)
        {
            ReadArticleDto newArticle = await _articleService.CreateArticleAsync(article);
            if (newArticle == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }

            return Ok(newArticle);
        }

        // DELETE: api/Article/5
        [HttpDelete]
        [Route("Article/{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticle([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleDto articleToDelete = await _articleService.GetArticleAsync(id);
            if (articleToDelete == null)
            {
                return BadRequest();
            }
            else
            {
                await _articleService.DeleteArticleAsync(id);
            }

            return NoContent();
        }
    }
}

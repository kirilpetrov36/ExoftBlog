using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Identity;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;

        public ArticleController(IArticleService articleService, UserManager<User> userManager)
        {
            _articleService = articleService;
            _userManager = userManager;
        }

        // GET: api/Articles
        [HttpGet]
        [Route("Articles")]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetArticles(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetArticlesAsync();
            return Ok(articles);
        }

        // GET: api/Articles/MostCommentable
        [HttpGet]
        [Route("Articles/MostCommentable")]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetMostCommentableArticles(CancellationToken token = default)
        {

            IEnumerable<ReadArticleDto> articles = await _articleService.GetMostCommentableArticlesAsync();
            return Ok(articles);
        }

        // GET: api/Article/MostLikeable
        [HttpGet]
        [Route("Articles/MostLikeable")]
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

        // GET: api/Article/User/:id
        [HttpGet]
        [Route("Articles/User/{userId:Guid}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadArticleDto>>> GetUserArticles(Guid userId, CancellationToken token = default)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }
            IEnumerable<ReadArticleDto> articles = await _articleService.GetUserArticlesAsync(userId, token);
            return Ok(articles);
        }

        // GET: api/Article/5
        [HttpGet]
        [Route("Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadFullArticleDto>> GetArticle([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleDto article = await _articleService.GetArticleAsync(id);
            Console.WriteLine(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // GET: api/SearchArticle
        [HttpGet]
        [Route("SearchArticle/{searchInput}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadFullArticleDto>>> SearchArticles([FromRoute] string searchInput, CancellationToken token = default)
        {
            IEnumerable<ReadArticleDto> articles = await _articleService.SearchArticles(searchInput, token);
            return Ok(articles);
        }

        //GET: api/FullArticle/5
        [HttpGet]
        [Route("FullArticle/{id:Guid}")]
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
        [Authorize]
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

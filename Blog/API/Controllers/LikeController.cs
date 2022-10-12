using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.LikeDto;
using Microsoft.AspNetCore.Authorization;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleLikeController : ControllerBase
    {
        private readonly IArticleLikeService _articleLikeService;

        public ArticleLikeController(IArticleLikeService articleLikeService)
        {
            _articleLikeService = articleLikeService;
        }

        // GET: api/ArticleLikes
        [HttpGet]
        [Route("ArticleLikes")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadArticleLikeDto>>> GetArticleLikes(CancellationToken token = default)
        {

            IEnumerable<ReadArticleLikeDto> articleLikes = await _articleLikeService.GetArticleLikesAsync();
            return Ok(articleLikes);
        }

        // GET: api/ArticleLike/5
        [HttpGet]
        [Route("ArticleLike/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadArticleLikeDto>> GetArticleLike([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadArticleLikeDto articleLike = await _articleLikeService.GetArticleLikeAsync(id);

            if (articleLike == null)
            {
                return BadRequest();
            }

            return Ok(articleLike);
        }

        // POST: api/ArticleLike
        [HttpPost]
        [Route("ArticleLike")]
        [Authorize]
        public async Task<ActionResult<ReadArticleLikeDto>> PostArticleLike([FromBody] CreateArticleLikeDto articleLike, CancellationToken token = default)
        {
            ReadArticleLikeDto newArticleLike = await _articleLikeService.CreateArticleLikeAsync(articleLike);
            if (newArticleLike == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                return Ok(newArticleLike);
            }      
        }

        // PATCH: api/ArticleLike/5
        [HttpPatch]
        [Route("ArticleLike/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadArticleLikeDto>> PatchArticleLike([FromRoute] Guid id, JsonPatchDocument<ArticleLike> articleLikeUpdates, CancellationToken token = default)
        {
            ReadArticleLikeDto articleLike = await _articleLikeService.GetArticleLikeAsync(id);
            if (articleLike == null)
            {
                return BadRequest();
            }
            ReadArticleLikeDto modifiedArticleLike = await _articleLikeService.PatchArticleLikeAsync(id, articleLikeUpdates);
            return Ok(modifiedArticleLike);
        }

        // GET: api/LikesAmount/Article/5
        [HttpGet]
        [Route("LikesAmount/Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<int?>> GetCommentLikesAmountAsync([FromRoute] Guid ArticleId, CancellationToken token = default)
        {
            int? result = await _articleLikeService.GetArticleLikesAmountAsync(ArticleId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }

    [Route("api")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;

        public CommentLikeController(ICommentLikeService commentLikeService)
        {
            _commentLikeService = commentLikeService;
        }

        // GET: api/CommentLikes
        [HttpGet]
        [Route("CommentLikes")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadCommentLikeDto>>> GetCommentLikes(CancellationToken token = default)
        {

            IEnumerable<ReadCommentLikeDto> commentLikes = await _commentLikeService.GetCommentLikesAsync();
            return Ok(commentLikes);
        }

        // GET: api/CommentLike/5
        [HttpGet]
        [Route("CommentLike/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadCommentLikeDto>> GetCommentLike([FromRoute] Guid id, CancellationToken token = default)
        {
            ReadCommentLikeDto commentLike = await _commentLikeService.GetCommentLikeAsync(id);

            if (commentLike == null)
            {
                return NotFound();
            }

            return Ok(commentLike);
        }

        // POST: api/CommentLike
        [HttpPost]
        [Route("CommentLike")]
        [Authorize]
        public async Task<ActionResult<ReadCommentLikeDto>> PostCommentLike([FromBody] CreateCommentLikeDto commentLike, CancellationToken token = default)
        {
            ReadCommentLikeDto newCommentLike = await _commentLikeService.CreateCommentLikeAsync(commentLike);
            if (newCommentLike == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                return Ok(newCommentLike);
            }
        }

        // PATCH: api/CommentLike/5
        [HttpPatch]
        [Route("CommentLike/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadCommentLikeDto>> PatchCommentLike([FromRoute] Guid id, JsonPatchDocument<CommentLike> commentLikeUpdates, CancellationToken token = default)
        {
            ReadCommentLikeDto commentLike = await _commentLikeService.GetCommentLikeAsync(id);
            if (commentLike == null)
            {
                return BadRequest();
            }
            ReadCommentLikeDto modifiedCommentLike = await _commentLikeService.PatchCommentLikeAsync(id, commentLikeUpdates);
            return Ok(modifiedCommentLike);
        }

        // GET: api/LikesAmount/Comment/5
        [HttpGet]
        [Route("LikesAmount/Comment/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<int?>> GetCommentLikesAmountAsync([FromRoute] Guid CommentId, CancellationToken token = default)
        {
            int? result = await _commentLikeService.GetCommentLikesAmountAsync(CommentId);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

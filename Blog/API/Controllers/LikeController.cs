using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }

        // GET: api/PostLikes
        [HttpGet]
        [Route("PostLikes")]
        public async Task<ActionResult<IEnumerable<ReadPostLikeDto>>> GetPostLikes()
        {

            IEnumerable<ReadPostLikeDto> postLikes = await _postLikeService.GetPostLikesAsync();
            return Ok(postLikes);
        }

        // GET: api/PostLike/5
        [HttpGet]
        [Route("PostLike/{id:long}")]
        public async Task<ActionResult<ReadPostLikeDto>> GetPostLike([FromRoute] long id)
        {
            ReadPostLikeDto postLike = await _postLikeService.GetPostLikeAsync(id);

            if (postLike == null)
            {
                return BadRequest();
            }

            return Ok(postLike);
        }

        // POST: api/PostLike
        [HttpPost]
        [Route("PostLike")]
        public async Task<ActionResult<ReadPostLikeDto>> PostPostLike([FromBody] CreatePostLikeDto postLike)
        {
            ReadPostLikeDto newPostLike = await _postLikeService.CreatePostLikeAsync(postLike);
            if (newPostLike == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                return Ok(newPostLike);
            }      
        }

        // UPDATE :api/PostLike/ChangeState/1
        [HttpPut("PostLike/ChangeState/{id:long}")]
        public async Task<IActionResult> ChangePostLikeState([FromRoute] long id, ChangeStateDto state)
        {
            ReadPostLikeDto postLikeToModify = await _postLikeService.GetPostLikeAsync(id);
            if (postLikeToModify == null)
            {
                return BadRequest();
            }
            else
            {
                CreatePostLikeDto modifiedPostLike = await _postLikeService.ChangePostLikeStateAsync(id, state);
                return Ok(modifiedPostLike);
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
        public async Task<ActionResult<IEnumerable<ReadCommentLikeDto>>> GetCommentLikes()
        {

            IEnumerable<ReadCommentLikeDto> commentLikes = await _commentLikeService.GetCommentLikesAsync();
            return Ok(commentLikes);
        }

        // GET: api/CommentLike/5
        [HttpGet]
        [Route("CommentLike/{id:long}")]
        public async Task<ActionResult<ReadCommentLikeDto>> GetCommentLike([FromRoute] long id)
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
        public async Task<ActionResult<ReadCommentLikeDto>> CommentCommentLike([FromBody] CreateCommentLikeDto commentLike)
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

        // UPDATE :api/CommentLike/ChangeState/1
        [HttpPut("CommentLike/ChangeState/{id:long}")]
        public async Task<IActionResult> ChangeCommentLikeState([FromRoute] long id, ChangeStateDto state)
        {
            ReadCommentLikeDto commentLikeToModify = await _commentLikeService.GetCommentLikeAsync(id);
            if (commentLikeToModify == null)
            {
                return NotFound();
            }
            else
            {
                CreateCommentLikeDto modifiedCommentLike = await _commentLikeService.ChangeCommentLikeStateAsync(id, state);
                return Ok(modifiedCommentLike);
            }
        }
    }
}

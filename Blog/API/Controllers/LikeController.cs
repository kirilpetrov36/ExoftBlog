using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ReadPostLikeDto>>> GetPostLikes()
        {

            IEnumerable<ReadPostLikeDto> postLikes = await _postLikeService.GetPostLikesAsync();
            return Ok(postLikes);
        }

        // GET: api/PostLikes/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ReadPostLikeDto>> GetPostLike([FromRoute] long id)
        {
            ReadPostLikeDto postLike = await _postLikeService.GetPostLikeAsync(id);

            if (postLike == null)
            {
                return NotFound();
            }

            return Ok(postLike);
        }

        // POST: api/PostLikes
        // To protect from overpostLikeing attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadPostLikeDto>> PostPostLike([FromBody] CreatePostLikeDto postLike)
        {
            ReadPostLikeDto newPostLike = await _postLikeService.CreatePostLikeAsync(postLike);
            return Ok(newPostLike);
        }

        // UPDATE :api/ChangePostLikeState/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> ChangePostLikeState([FromRoute] long id, ChangeStateDto state)
        {
            ReadPostLikeDto postLikeToModify = await _postLikeService.GetPostLikeAsync(id);
            if (postLikeToModify == null)
            {
                return NotFound();
            }
            else
            {
                CreatePostLikeDto modifiedPostLike = await _postLikeService.ChangePostLikeStateAsync(id, state);
                return Ok(modifiedPostLike);
            }
        }

    }

    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ReadCommentLikeDto>>> GetCommentLikess()
        {

            IEnumerable<ReadCommentLikeDto> commentLikes = await _commentLikeService.GetCommentLikesAsync();
            return Ok(commentLikes);
        }

        // GET: api/CommentLikes/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ReadCommentLikeDto>> GetCommentLike([FromRoute] long id)
        {
            ReadCommentLikeDto commentLike = await _commentLikeService.GetCommentLikeAsync(id);

            if (commentLike == null)
            {
                return NotFound();
            }

            return Ok(commentLike);
        }

        // POST: api/CommentLikes
        // To protect from overcommentLikeing attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadCommentLikeDto>> CommentCommentLike([FromBody] CreateCommentLikeDto commentLike)
        {
            ReadCommentLikeDto newCommentLike = await _commentLikeService.CreateCommentLikeAsync(commentLike);
            return Ok(newCommentLike);
        }

        // UPDATE :api/ChangeCommentLikeState/1
        [HttpPut("{id:long}")]
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

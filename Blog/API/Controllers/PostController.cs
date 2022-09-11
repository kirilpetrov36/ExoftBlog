using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/Posts
        [HttpGet]
        [Route("Posts")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetPosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetPostsAsync();
            return Ok(posts);
        }

        // GET: api/Posts/MostCommentable
        [HttpGet]
        [Route("Posts/MostCommentable")]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetMostCommentablePosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetMostCommentablePostsAsync();
            return Ok(posts);
        }

        // GET: api/Post/MostLikeable
        [HttpGet]
        [Route("Posts/MostLikeable")]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetMostLikeablePosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetMostLikeablePostsAsync();
            return Ok(posts);
        }

        // GET: api/Post/5
        [HttpGet]
        [Route("Post/{id:long}")]
        public async Task<ActionResult<ReadPostDto>> GetPost([FromRoute] long id)
        {
            ReadPostDto post = await _postService.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // GET: api/Post/5/Comments
        [HttpGet]
        [Route("Post/{id:long}/Comments")]
        public async Task<ActionResult<ReadPostCommentsDto>> GetPostComments([FromRoute] long id)
        {
            ReadPostCommentsDto post = await _postService.GetPostCommentsAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // GET: api/Post/5/Likes
        [HttpGet]
        [Route("Post/{id:long}/Likes")]
        public async Task<ActionResult<ReadPostLikesDto>> GetPostlikes([FromRoute] long id)
        {
            ReadPostLikesDto post = await _postService.GetPostLikesAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Post/5
        [HttpPut]
        [Route("Post/{id:long}")]
        public async Task<IActionResult> PutPost([FromRoute]long id, [FromBody] CreatePostDto post)
        {
            ReadPostDto postToModify = await _postService.GetPostAsync(id);
            if (postToModify == null)
            {
                return BadRequest();
            }
            else
            {
                CreatePostDto newPost = await _postService.UpdatePostAsync(id, post);
                return Ok(newPost);
            }
        }

        // POST: api/Post
        [HttpPost]
        [Route("Post")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReadPostDto>> PostPost([FromBody]CreatePostDto post)
        {
            ReadPostDto newPost = await _postService.CreatePostAsync(post);
            if (newPost == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                return Ok(newPost);
            }
        }

        // DELETE: api/Post/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePost([FromRoute]long id)
        {
            ReadPostDto postToDelete = await _postService.GetPostAsync(id);
            if (postToDelete == null)
            {
                return BadRequest();
            }
            else
            {
                await _postService.DeletePostAsync(id);
            }

            return NoContent();
        }

        //UPDATE :api/Post/ChangeState/1
        [HttpPut("Post/ChangeState/{id:long}")]
        public async Task<IActionResult> ChangePostState([FromRoute] long id, ChangeStateDto state)
        {
            ReadPostDto postToModify = await _postService.GetPostAsync(id);
            if (postToModify == null)
            {
                return BadRequest();
            }
            else
            {
                CreatePostDto modifiedPost = await _postService.ChangePostStateAsync(id, state);
                return Ok(modifiedPost);
            }
        }

    }
}

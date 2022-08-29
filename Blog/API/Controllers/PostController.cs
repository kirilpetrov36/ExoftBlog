using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetPosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetPostsAsync();
            return Ok(posts);
        }

        // GET: api/Post/MostCommentable
        [HttpGet]
        [Route("MostCommentable")]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetMostCommentablePosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetMostCommentablePostsAsync();
            return Ok(posts);
        }

        // GET: api/Post/MostLikeable
        [HttpGet]
        [Route("MostLikeable")]
        public async Task<ActionResult<IEnumerable<ReadPostDto>>> GetMostLikeablePosts()
        {

            IEnumerable<ReadPostDto> posts = await _postService.GetMostLikeablePostsAsync();
            return Ok(posts);
        }

        // GET: api/Post/5
        [HttpGet("{id:long}")]
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
        [Route("{id:long}/Comments")]
        public async Task<ActionResult<ReadPostCommentsDto>> GetPostComments([FromRoute] long id)
        {
            ReadPostCommentsDto post = await _postService.GetPostCommentsAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // GET: api/Post/5/Comments
        [HttpGet]
        [Route("{id:long}/PostLikes")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:long}")]
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
         
            /*
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadPostDto>> PostPost([FromBody]CreatePostDto post)
        {
            ReadPostDto newPost = await _postService.CreatePostAsync(post);
            return Ok(newPost);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletePost([FromRoute]long id)
        {
            //if (_context.Posts == null)
            //{
            //    return NotFound();
            //}
            ReadPostDto postToDelete = await _postService.GetPostAsync(id);
            if (postToDelete == null)
            {
                return NotFound();
            }
            else
            {
                await _postService.DeletePostAsync(id);
            }

            return NoContent();
        }

        //UPDATE :api/ChangePostState/1
        [HttpPut("ChangeState/{id:long}")]
        public async Task<IActionResult> ChangePostState([FromRoute] long id, ChangeStateDto state)
        {
            ReadPostDto postToModify = await _postService.GetPostAsync(id);
            if (postToModify == null)
            {
                return NotFound();
            }
            else
            {
                CreatePostDto modifiedPost = await _postService.ChangePostStateAsync(id, state);
                return Ok(modifiedPost);
            }
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/Comments
        [HttpGet]
        [Route("Coments")]
        public async Task<ActionResult<IEnumerable<ReadCommentDto>>> GetComments()
        {

            IEnumerable<ReadCommentDto> comments = await _commentService.GetCommentsAsync();
            return Ok(comments);
        }

        // GET: api/Comment/5
        [HttpGet]
        [Route("Comment/{id:long}")]
        public async Task<ActionResult<ReadCommentDto>> GetComment([FromRoute] long id)
        {
            ReadCommentDto comment = await _commentService.GetCommentAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // GET: api/Comment/5/Likes
        [HttpGet]
        [Route("Comment/{id:long}/Likes")]
        public async Task<ActionResult<ReadCommentLikesDto>> GetCommentLikes([FromRoute] long id)
        {
            ReadCommentLikesDto comment = await _commentService.GetLikesAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // GET: api/Comment/5/Childs
        [HttpGet]
        [Route("Comment/{id:long}/Childs")]
        public async Task<ActionResult<ReadCommentChildsDto>> GetCommentChilds([FromRoute] long id)
        {
            ReadCommentChildsDto comment = await _commentService.GetCommentChildsAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comment/5
        [HttpPut]
        [Route("Comment/{id:long}")]
        public async Task<IActionResult> PutComment([FromRoute] long id, [FromBody] CreateCommentDto comment)
        {
            ReadCommentDto commentToModify = await _commentService.GetCommentAsync(id);
            if (commentToModify == null)
            {
                return BadRequest();
            }
            else
            {
                CreateCommentDto newComment = await _commentService.UpdateCommentAsync(id, comment);
                return Ok(newComment);
            }
        }

        // POST: api/Comment
        [HttpPost]
        [Route("Comment")]
        public async Task<ActionResult<ReadCommentDto>> PostComment([FromBody] CreateCommentDto comment)
        {
            ReadCommentDto newComment = await _commentService.CreateCommentAsync(comment);
            if (newComment == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            else
            {
                return Ok(newComment);
            }
        }

        // DELETE: api/Comment/5
        [HttpDelete]
        [Route("Comment/{id:long}")]
        public async Task<IActionResult> DeleteComment([FromRoute] long id)
        {
            ReadCommentDto commentToModify = await _commentService.GetCommentAsync(id);
            if (commentToModify == null)
            {
                return BadRequest();
            }
            ReadCommentDto commentToDelete = await _commentService.GetCommentAsync(id);
            if (commentToDelete == null)
            {
                return BadRequest();
            }
            else
            {
                await _commentService.DeleteCommentAsync(id);
            }

            return NoContent();
        }

        //UPDATE :api/ChangeCommentState/1
        [HttpPut]
        [Route("ChangeCommentState/{id:long}")]
        public async Task<IActionResult> ChangeCommentState([FromRoute] long id, ChangeStateDto state)
        {
            ReadCommentDto commentToModify = await _commentService.GetCommentAsync(id);
            if (commentToModify == null)
            {
                return NotFound();
            }
            else
            {
                CreateCommentDto modifiedComment = await _commentService.ChangeCommentStateAsync(id, state);
                return Ok(modifiedComment);
            }
        }
    }
}

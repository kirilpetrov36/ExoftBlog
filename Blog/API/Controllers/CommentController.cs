using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.CommentDto;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCommentDto>>> GetCommentss()
        {

            IEnumerable<ReadCommentDto> comments = await _commentService.GetCommentsAsync();
            return Ok(comments);
        }

        // GET: api/Comment/5
        [HttpGet("{id:long}")]
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
        [Route("{id:long}/Likes")]
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
        [Route("{id:long}/Childs")]
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
        // To protect from overcommenting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:long}")]
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

            /*
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/
        }

        // POST: api/Comment
        // To protect from overcommenting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReadCommentDto>> PostComment([FromBody] CreateCommentDto comment)
        {
            ReadCommentDto newComment = await _commentService.CreateCommentAsync(comment);
            return Ok(newComment);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteComment([FromRoute] long id)
        {
            //if (_context.Comments == null)
            //{
            //    return NotFound();
            //}
            ReadCommentDto commentToDelete = await _commentService.GetCommentAsync(id);
            if (commentToDelete == null)
            {
                return NotFound();
            }
            else
            {
                await _commentService.DeleteCommentAsync(id);
            }

            return NoContent();
        }

        // UPDATE :api/ChangeCommentState/1
        //[HttpPut("{id:long}")]
        //public async Task<IActionResult> ChangeCommentState([FromRoute] long id, ChangeStateDto state)
        //{
        //    ReadCommentDto commentToModify = await _commentService.GetCommentAsync(id);
        //    if (commentToModify == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        CreateCommentDto modifiedComment = await _commentService.ChangeCommentStateAsync(id, state);
        //        return Ok(modifiedComment);
        //    }
        //}
    }
}

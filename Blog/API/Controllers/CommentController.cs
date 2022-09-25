using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.CommentDto;
using Microsoft.AspNetCore.Authorization; 
using Blog.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReadCommentDto>>> GetComments()
        {

            IEnumerable<ReadCommentDto> comments = await _commentService.GetCommentsAsync();
            return Ok(comments);
        }

        // GET: api/Comment/5
        [HttpGet]
        [Route("Comment/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadCommentDto>> GetComment([FromRoute] Guid id)
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
        [Route("Comment/{id:Guid}/Likes")]
        [Authorize]
        public async Task<ActionResult<ReadCommentLikesDto>> GetCommentLikes([FromRoute] Guid id)
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
        [Route("Comment/{id:Guid}/Childs")]
        [Authorize]
        public async Task<ActionResult<ReadCommentChildsDto>> GetCommentChilds([FromRoute] Guid id)
        {
            ReadCommentChildsDto comment = await _commentService.GetCommentChildsAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PATCH: api/Comment/5
        [HttpPatch]
        [Route("Comment/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<ReadCommentDto>> PatchComment([FromRoute] Guid id, JsonPatchDocument<Comment> commentUpdates)
        {
            ReadCommentDto comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
            {
                return BadRequest();
            }
            ReadCommentDto modifiedComment = await _commentService.PatchCommentAsync(id, commentUpdates);
            return Ok(modifiedComment);
        }

        // PUT: api/Comment/5
        [HttpPut]
        [Route("Comment/{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> PutComment([FromRoute] Guid id, [FromBody] CreateCommentDto comment)
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
        [Authorize]
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
        [Route("Comment/{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid Id)
        {
            ReadCommentDto commentToModify = await _commentService.GetCommentAsync(Id);
            if (commentToModify == null)
            {
                return BadRequest();
            }
            ReadCommentDto commentToDelete = await _commentService.GetCommentAsync(Id);
            if (commentToDelete == null)
            {
                return BadRequest();
            }
            else
            {
                await _commentService.DeleteCommentAsync(Id);
            }

            return NoContent();
        }

        // GET: api/CommentAmount/Article/5
        [HttpGet]
        [Route("CommentAmount/Article/{id:Guid}")]
        [Authorize]
        public async Task<ActionResult<int?>> GetArticleCommentsAmount([FromRoute] Guid Id)
        {   
            int? result = await _commentService.GetArticleCommentsAmount(Id);
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

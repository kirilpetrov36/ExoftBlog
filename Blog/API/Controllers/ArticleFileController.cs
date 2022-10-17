using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleFileController : ControllerBase
    {
        private readonly IArticleFileService _fileService;

        public ArticleFileController (IArticleFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("ArticleFiles")]
        [HttpGet]
        public async Task<ActionResult<List<ArticleFile>>> GetPostImages([FromBody]Guid articleId, CancellationToken token = default)
        {
            List <ArticleFile> result = await _fileService.GetArticleFiles(articleId);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("ArticleFile")]
        [HttpGet]
        public async Task<ActionResult<ArticleFile>> GetFileById([FromBody] Guid FileId, CancellationToken token = default)
        {
            ArticleFile result = await _fileService.GetFileById(FileId);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("ArticleFile/{postId}")]
        [HttpPost]
        public async Task<ActionResult<ArticleFile>> UploadFile(IFormFile file, Guid postId, CancellationToken token = default)
        { 
            ArticleFile dataFile = await _fileService.UploadFilesAsync(file, postId);
            if (dataFile == null)
            {
                return BadRequest();
            }
            return Ok(dataFile);
        }

        [Route("ArticleFile/{FileId}")]
        [HttpDelete]
        public async Task DeleteFile(Guid FileId, CancellationToken token = default)
        {
            await _fileService.RemoveFileAsync(FileId);
        }
    }
}

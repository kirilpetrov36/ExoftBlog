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
        public async Task<ActionResult<List<ArticleFile>>> GetPostImages(Guid articleId, CancellationToken token = default)
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
        public async Task<ActionResult<ArticleFile>> GetFileById(Guid FileId, CancellationToken token = default)
        {
            ArticleFile result = await _fileService.GetFileById(FileId);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("ArticleFile")]
        [HttpPost]
        public async Task<ActionResult<List<ArticleFile>>> UploadFile(ICollection<IFormFile> files, Guid postId, CancellationToken token = default)
        { 
            List<ArticleFile> dataFiles = await _fileService.UploadFilesAsync(files, postId);
            if (dataFiles == null)
            {
                return BadRequest();
            }
            return Ok(dataFiles);
        }

        [Route("ArticleFile")]
        [HttpDelete]
        public async Task DeleteFile(Guid FileId, CancellationToken token = default)
        {
            await _fileService.RemoveFileAsync(FileId);
        }
    }
}

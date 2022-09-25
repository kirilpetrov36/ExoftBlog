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

        [Route("PostFiles")]
        [HttpGet]
        public async Task<ActionResult<List<ArticleFile>>> GetPostImages(Guid postId)
        {
            List <ArticleFile> result = await _fileService.GetPostImages(postId);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("File")]
        [HttpGet]
        public async Task<ActionResult<ArticleFile>> GetFileById(Guid FileId)
        {
            ArticleFile result = await _fileService.GetFileById(FileId);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("File")]
        [HttpPost]
        public async Task<ActionResult<List<ArticleFile>>> UploadFile(ICollection<IFormFile> files, Guid postId)
        { 
            List<ArticleFile> dataFiles = await _fileService.UploadFilesAsync(files, postId);
            if (dataFiles == null)
            {
                return BadRequest();
            }
            return Ok(dataFiles);
        }

        [Route("File")]
        [HttpDelete]
        public async Task DeleteFile(Guid FileId)
        {
            await _fileService.RemoveFileAsync(FileId);
        }
    }
}

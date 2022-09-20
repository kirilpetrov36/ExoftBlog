using Microsoft.AspNetCore.Mvc;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController (IFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("PostFiles")]
        [HttpGet]
        public async Task<ActionResult<List<DataFile>>> GetPostImages(Guid postId)
        {
            List <DataFile> result = await _fileService.GetPostImages(postId);
            if(result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("File")]
        [HttpGet]
        public async Task<ActionResult<DataFile>> GetFileById(Guid FileId)
        {
            DataFile result = await _fileService.GetFileById(FileId);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("File")]
        [HttpPost]
        public async Task<ActionResult<List<DataFile>>> UploadFile(ICollection<IFormFile> files, Guid postId)
        { 
            List<DataFile> dataFiles = await _fileService.UploadFilesAsync(files, postId);
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

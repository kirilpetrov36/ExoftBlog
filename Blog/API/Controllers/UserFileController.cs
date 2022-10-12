using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserFileController : ControllerBase
    {
        private readonly IUserFileService _fileService;

        public UserFileController(IUserFileService fileService)
        {
            _fileService = fileService;
        }

        [Route("UserFiles")]
        [HttpGet]
        public async Task<ActionResult<List<UserFile>>> GetFiles(CancellationToken token = default)
        {
            List<UserFile> result = await _fileService.GetFiles();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("UserFile")]
        [HttpGet]
        public async Task<ActionResult<UserFile>> GetFileById(Guid FileId, CancellationToken token = default)
        {
            UserFile result = await _fileService.GetFileById(FileId);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [Route("UserFile")]
        [HttpPost]
        public async Task<ActionResult<List<UserFile>>> UploadFile(ICollection<IFormFile> files, CancellationToken token = default)
        {
            List<UserFile> userFiles = await _fileService.UploadFilesAsync(files);
            if (userFiles == null)
            {
                return BadRequest();
            }
            return Ok(userFiles);
        }

        [Route("UserFile")]
        [HttpDelete]
        public async Task DeleteFile(Guid FileId, CancellationToken token = default)
        {
            await _fileService.RemoveFileAsync(FileId);
        }
    }
}

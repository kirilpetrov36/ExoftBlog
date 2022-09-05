using Microsoft.AspNetCore.Mvc;
using Blog.BLL.DTO.File;
using Blog.BLL.Services.Interfaces;

namespace Blog.API.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController (IFileService fileService)
        {
            _fileService = fileService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetFile([FromQuery] GetFileDto getFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ReadFileDto fileData = await _fileService.GetFileAsync(getFile);
        //        if (fileData != null)
        //        {
        //            return Ok(fileData);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> InsertFile([FromForm] IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                ReadFileDto fileData = await _fileService.InsertFileAsync(formFile);
                return Ok(fileData);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

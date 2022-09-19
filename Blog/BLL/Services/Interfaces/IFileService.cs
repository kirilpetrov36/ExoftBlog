using Blog.BLL.DTO.File;

namespace Blog.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<ReadFileDto> InsertFileAsync(IFormFile formFile);
    }
}

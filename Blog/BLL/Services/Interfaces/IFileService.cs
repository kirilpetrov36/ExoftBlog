using Blog.BLL.DTO.File;
using Blog.BLL.Services.ExternalServices;
using Blog.DAL.Entities;

namespace Blog.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<DataFile>> UploadFilesAsync(ICollection<IFormFile> files, Guid postId);
        Task<DataFile> GetFileById(Guid FileId);
        byte[] ReadFully(Stream input);
        Task<List<DataFile>> GetPostImages(Guid postId);
        Task<bool> RemoveFileAsync(Guid FileId);
    }
}

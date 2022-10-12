using Blog.DAL.Entities;

namespace Blog.BLL.Services.Interfaces
{
    public interface IUserFileService
    {
        Task<List<UserFile>> UploadFilesAsync(ICollection<IFormFile> files);
        byte[] ReadFully(Stream input);
        Task<bool> RemoveFileAsync(Guid FileId);
        Task<List<UserFile>> GetFiles();
        Task<UserFile> GetFileById(Guid FileId);
    }
}

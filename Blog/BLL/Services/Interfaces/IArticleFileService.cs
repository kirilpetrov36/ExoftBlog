using Blog.BLL.DTO.File;
using Blog.BLL.Services.ExternalServices;
using Blog.DAL.Entities;

namespace Blog.BLL.Services.Interfaces
{
    public interface IArticleFileService
    {
        Task<List<ArticleFile>> UploadFilesAsync(ICollection<IFormFile> files, Guid postId);
        Task<ArticleFile> GetFileById(Guid FileId);
        byte[] ReadFully(Stream input);
        Task<List<ArticleFile>> GetPostImages(Guid postId);
        Task<bool> RemoveFileAsync(Guid FileId);
    }
}

using Blog.DAL.Entities;

namespace Blog.BLL.Services.Interfaces
{
    public interface IArticleFileService
    {
        Task<ArticleFile> UploadFilesAsync(IFormFile file, Guid postId);
        Task<ArticleFile> GetFileById(Guid FileId);
        byte[] ReadFully(Stream input);
        Task<List<ArticleFile>> GetArticleFiles(Guid postId);
        Task<bool> RemoveFileAsync(Guid FileId);
    }
}

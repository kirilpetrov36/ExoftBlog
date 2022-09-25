using Blog.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IArticleLikeRepository : IRepository<ArticleLike>
    {
        Task<int?> GetArticleLikesAmountAsync(Guid ArticleId);   
    }

    public interface ICommentLikeRepository : IRepository<CommentLike>
    {
        Task<int?> GetCommentLikesAmountAsync(Guid CommentId);
    }
}

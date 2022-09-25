using Blog.DAL.Entities;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.ArticleFileDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadFullArticleDto : BaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<ReadArticleLikeDto>? Likes { get; set; }
        public List<ReadCommentDto>? Comments { get; set; }
        public List<ReadArticleFileDto>? ArticleFiles { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; }
    }
}

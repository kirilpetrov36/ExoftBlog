using Blog.BLL.DTO.ArticleDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.LikeDto
{
    public class ReadLikeDto : BaseDto
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ReadArticleLikeDto : ReadLikeDto
    {     
        public ReadUserDto User { get; set; }
        public ReadArticleDto Article { get; set; }
    }

    public class ReadCommentLikeDto : ReadLikeDto
    {
        public ReadUserDto User { get; set; }
        public ReadCommentDto Comment { get; set; }
    }
}

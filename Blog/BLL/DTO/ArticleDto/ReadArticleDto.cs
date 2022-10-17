using Blog.BLL.DTO.ArticleFileDto;
using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.UserDto;

namespace Blog.BLL.DTO.ArticleDto
{
    public class ReadArticleDto : BaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ReadUserDto User { get; set; }
        public int LikesAmount { get; set; }
        public int CommentsAmount { get; set; }
        public List<ReadArticleFileDto>? ArticleFiles { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVerified { get; set; }
    }
}

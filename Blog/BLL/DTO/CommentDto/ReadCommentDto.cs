using Blog.BLL.DTO.ArticleDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.CommentDto
{
    public class ReadCommentDto : BaseDto
    {
        public string Text { get; set; }
        public ReadUserDto User { get; set; }
        public ReadArticleDto Article { get; set; }
        public ReadCommentDto? ParentComment { get; set; }
        public List<ReadCommentDto>? ChildComments { get; set; }
        public List<ReadCommentLikeDto>? Likes { get; set; }
    }
}

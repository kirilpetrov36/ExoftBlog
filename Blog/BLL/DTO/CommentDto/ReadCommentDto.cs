using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.LikeDto;

namespace Blog.BLL.DTO.CommentDto
{
    public class ReadCommentDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public ReadUserDto User { get; set; }
        public ReadPostDto Post { get; set; }
        public ReadCommentDto? ParentComment { get; set; }
        public List<ReadCommentDto>? ChildComments { get; set; }
        public List<ReadCommentLikeDto>? Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

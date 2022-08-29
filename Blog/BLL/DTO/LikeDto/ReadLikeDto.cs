using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.LikeDto
{
    public class ReadLikeDto
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isDeleted { get; set; }
    }

    public class ReadPostLikeDto : ReadLikeDto
    {     
        public ReadUserDto User { get; set; }
        public ReadPostDto Post { get; set; }
    }

    public class ReadCommentLikeDto : ReadLikeDto
    {
        public ReadUserDto User { get; set; }
        public ReadCommentDto Comment { get; set; }
    }
}

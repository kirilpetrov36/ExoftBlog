using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

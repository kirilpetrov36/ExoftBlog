using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserCommentsDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ReadCommentDto>? Comments { get; set; }
    }
}

using Blog.BLL.DTO.UserFileDto;

namespace Blog.BLL.DTO.UserDto
{
    public class ReadUserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ReadUserFileDto> Files { get; set; }
    }
}

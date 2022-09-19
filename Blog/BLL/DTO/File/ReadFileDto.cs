namespace Blog.BLL.DTO.File
{
    public class ReadFileDto : BaseDto
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public string AltUrl { get; set; }
        public string ContentType { get; set; }
    }
}

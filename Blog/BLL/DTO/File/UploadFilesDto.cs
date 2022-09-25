namespace Blog.BLL.DTO.File
{
    public class UploadFilesDto
    {
        public ICollection<IFormFile> files { get; set; }
        public Guid PostId { get; set; }
    }
}

using Blog.BLL.DTO.CommentDto;

namespace Blog.BLL.DTO.PostDto
{
    public class ReadPostCommentsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<ReadCommentDto>? Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

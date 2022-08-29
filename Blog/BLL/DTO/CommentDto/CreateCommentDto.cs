namespace Blog.BLL.DTO.CommentDto
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public long? ParentCommentId { get; set; }
    }
}

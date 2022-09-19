namespace Blog.BLL.DTO.CommentDto
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}

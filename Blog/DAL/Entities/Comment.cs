namespace Blog.DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public User User { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public List<CommentLike>? Likes { get; set; }
        public List<Comment>? ChildComments { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

    }
}

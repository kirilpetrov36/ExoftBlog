namespace Blog.DAL.Entities
{
    public class ArticleLike : BaseEntity
    {
        //public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CommentLike : BaseEntity
    {
        //public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        public bool IsDeleted { get; set; }
    }
}

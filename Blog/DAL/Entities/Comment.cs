using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class Comment : IDate
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public List<CommentLike>? Likes { get; set; }
        public List<Comment>? ChildComments { get; set; }
        public long? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class PostLike : ILike
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isEnable { get; set; }
    }

    public class CommentLike : ILike
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isEnable { get; set; }
    }
}

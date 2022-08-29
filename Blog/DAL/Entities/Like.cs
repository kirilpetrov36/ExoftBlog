using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class PostLike : BaseEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }

    public class CommentLike : BaseEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}

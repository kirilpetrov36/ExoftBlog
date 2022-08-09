using Blog.DAL.Models.Interfaces;

namespace Blog.DAL.Models
{
    public class PostLike : ILike
    {
        public long Id { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isEnable { get; set; }
    }

    public class CommentLike : ILike
    {
        public long Id { get; set; }
        public User User { get; set; }
        public Comment Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isEnable { get; set; }
    }
}

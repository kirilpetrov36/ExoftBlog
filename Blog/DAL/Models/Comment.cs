using Blog.DAL.Models.Interfaces;

namespace Blog.DAL.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public List<CommentLike> CommentLikes { get; set; }
        public Comment ParentRef { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}

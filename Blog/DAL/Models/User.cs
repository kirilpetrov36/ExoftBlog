using Blog.DAL.Models.Interfaces;

namespace Blog.DAL.Models
{
    public class User : IDate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Comment> Comments { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<CommentLike> CommentLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

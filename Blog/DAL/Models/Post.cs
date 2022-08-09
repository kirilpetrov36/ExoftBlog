using Blog.DAL.Models.Interfaces;

namespace Blog.DAL.Models
{
    public class Post : IDate
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Image> Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}

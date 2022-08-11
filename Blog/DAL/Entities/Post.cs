using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class Post : IDate
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<PostLike>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Image>? Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}

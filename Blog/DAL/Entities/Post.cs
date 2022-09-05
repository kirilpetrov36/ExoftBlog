using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class Post : BaseEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<PostLike>? Likes { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<MediaFile>? MediaFiles { get; set; }

    }
}

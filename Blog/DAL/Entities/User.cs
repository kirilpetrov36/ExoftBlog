using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class User : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<PostLike>? PostLikes { get; set; }
        public List<CommentLike>? CommentLikes { get; set; }
    }
}

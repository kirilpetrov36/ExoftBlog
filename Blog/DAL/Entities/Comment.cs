using Blog.DAL.Entities.Interfaces;
using Blog.DAL.Entities;

namespace Blog.DAL.Entities
{
    public class Comment : BaseEntity
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public List<CommentLike>? Likes { get; set; }
        public List<Comment>? ChildComments { get; set; }
        public long? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

    }
}

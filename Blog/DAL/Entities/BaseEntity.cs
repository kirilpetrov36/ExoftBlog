using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class BaseEntity : IDate
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}

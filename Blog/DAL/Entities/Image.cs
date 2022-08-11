using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class Image : IDate
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string AltUrl { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

using Blog.DAL.Entities.Interfaces;

namespace Blog.DAL.Entities
{
    public class Image : BaseEntity
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string AltUrl { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}

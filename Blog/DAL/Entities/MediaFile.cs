using Blog.DAL.Enums;

namespace Blog.DAL.Entities
{
    public class MediaFile : BaseEntity
    {
        public long Id { get; set; }
        public string FilePath { get; set; }
        public string AltName { get; set; }
        public ContentType ContentType { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}

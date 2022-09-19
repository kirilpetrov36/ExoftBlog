using Blog.DAL.Enums;

namespace Blog.DAL.Entities
{
    public class MediaFile : BaseEntity
    {
        public string FilePath { get; set; }
        public string AltName { get; set; }
        public ContentType ContentType { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}

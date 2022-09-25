namespace Blog.DAL.Entities
{
    public class DataFile : BaseEntity
    {
        public string Url { get; set; }
        public string BlobName { get; set; }
        //public string AltName { get; set; }
        //public ContentType ContentType { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}

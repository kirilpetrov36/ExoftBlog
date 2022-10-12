namespace Blog.DAL.Entities
{
    public class UserFile : BaseEntity
    {
        public string Url { get; set; }
        public string BlobName { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

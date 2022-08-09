using Blog.DAL.Models.Interfaces;

namespace Blog.DAL.Models
{
    public class Image : IDate
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string AltUrl { get; set; }
        public Post PostRef { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

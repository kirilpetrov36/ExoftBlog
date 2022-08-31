using System;
using System.Text.Json.Serialization;

namespace Blog.DAL.Entities
{
    public class RefreshToken
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public virtual bool IsExpired => DateTime.UtcNow >= Expires;
    }
}

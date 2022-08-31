using System.Text.Json.Serialization;
using Blog.DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blog.DAL.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<PostLike>? PostLikes { get; set; }
        public List<CommentLike>? CommentLikes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool IsDeleted { get; set; }
    }
}

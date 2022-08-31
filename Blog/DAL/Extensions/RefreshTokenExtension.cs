using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class RefreshTokenExtension
    {
        public static void BuildRefreshTokenModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<RefreshToken>();
        }
    }
}

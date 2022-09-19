using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class UserExtension
    {
        public static void BuildUserModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(p => p.FirstName)
                .HasMaxLength(32)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(p => p.LastName)
                .HasMaxLength(32)
                .IsRequired();
        }
    }
}

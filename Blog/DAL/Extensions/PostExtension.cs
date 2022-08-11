using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class PostExtension
    {
        public static void BuildPostModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Post>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Post>()
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(10000);

            modelBuilder.Entity<Post>()
                .Property(p => p.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Post>()
               .Property(p => p.UpdatedAt)
               .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

        }

    }
}

using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class ImageExtension
    {
        public static void BuildImageModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Image>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Image>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<Image>()
                .HasOne(p => p.Post)
                .WithMany(p => p.Images)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();


        }
    }
}

using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;

namespace Blog.DAL.Extensions
{
    public static class MediaFileExtension
    {
        public static void BuildMediaFileModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<MediaFile>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MediaFile>()
                .Property(p => p.FilePath)
                .IsRequired();

            modelBuilder.Entity<MediaFile>()
                .HasOne(p => p.Post)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();


        }
    }
}

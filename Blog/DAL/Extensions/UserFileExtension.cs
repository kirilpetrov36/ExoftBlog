using Blog.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Extensions
{
    public static class UserFileExtension
    {
        public static void BuildUserFileModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFile>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<UserFile>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserFile>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder.Entity<UserFile>()
                .HasOne(p => p.User)
                .WithMany(p => p.Files)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}

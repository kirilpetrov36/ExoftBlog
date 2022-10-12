using Blog.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL.Extensions
{
    public static class UserSubscriptionExtension
    {
        public static void BuildUserSubscriptionModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSubscription>()
                .HasKey(us => new { us.UserId, us.UserToSubscribeId });

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserSubscription>()
            //    .HasOne(us => us.FollowedUser)
            //    .WithMany(u => u.UserSubscriptions)
            //    .HasForeignKey(us => us.FollowedUserId);
        }
    }
}

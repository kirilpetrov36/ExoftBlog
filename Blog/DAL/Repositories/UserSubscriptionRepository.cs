using Blog.DAL.Entities;
using Blog.DAL.Repositories.Interfaces;

namespace Blog.DAL.Repositories
{
    public class UserSubscriptionRepository: Repository<UserSubscription>, IUserSubscriptionRepository
    {
        private readonly AppDbContext _context;
        public UserSubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Delete(UserSubscription userSubscription)
        {
            _context.Set<UserSubscription>().Remove(userSubscription);
        }
    }
}

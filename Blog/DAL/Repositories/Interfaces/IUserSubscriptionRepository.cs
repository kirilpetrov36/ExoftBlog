using Blog.DAL.Entities;

namespace Blog.DAL.Repositories.Interfaces
{
    public interface IUserSubscriptionRepository: IRepository<UserSubscription>
    {
        void Delete(UserSubscription subscription);
    }
}

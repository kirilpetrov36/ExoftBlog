using AutoMapper;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;
using Blog.DAL.UnitOfWork;

namespace Blog.BLL.Services
{
    public class UserSubscriptionService: IUserSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public UserSubscriptionService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<UserSubscriptionDto> CreateUserSubscriptionAsync(Guid userToSubscribeId, CancellationToken token = default)
        {
            UserSubscription userSubscription = new() { UserId = _accountService.GetUserId(), UserToSubscribeId = userToSubscribeId };
            _logger.LogInformation("User - {userId} subscribe to user - {articleId}", userSubscription.UserId, userSubscription.UserToSubscribeId);
            _unitOfWork.UserSubscriptionRepository.CreateAsync(userSubscription);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<UserSubscriptionDto>(userSubscription);
        }

        public async Task DeleteUserSubscriptionAsync(Guid userToSubscribeId, CancellationToken token = default)
        {
            UserSubscription userSubscription = new() { UserId = _accountService.GetUserId(), UserToSubscribeId = userToSubscribeId };
            _logger.LogInformation("Delet User - {userId} subscribe to user - {articleId}", userSubscription.UserId, userSubscription.UserToSubscribeId);      
            _unitOfWork.UserSubscriptionRepository.Delete(userSubscription);
            await _unitOfWork.SaveChanges();
        }
    }
}

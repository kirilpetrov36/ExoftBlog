using Blog.BLL.Services.Interfaces;
using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.UserDto;
using Blog.BLL.DTO;
using Blog.DAL.Entities;
using AutoMapper;

namespace Blog.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadUserDto> CreateUserAsync(CreateUserDto item, CancellationToken token = default)
        {
            _logger.LogInformation("User creation with name - {name}", item.Name);

            DateTime timeNow = DateTime.UtcNow;
            User user = _mapper.Map<User>(item);
            user.CreatedAt = timeNow;
            user.UpdatedAt = timeNow;
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReadUserDto>(user);
        }

        public async Task DeleteUserAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete user with id - {id}", id);
            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<CreateUserDto> ChangeUserStateAsync(long id, ChangeStateDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Disable user with id - {id}", id);
            User userToDisable = await _unitOfWork.UserRepository.GetAsync(id);
            userToDisable.IsDeleted = item.IsDeleted;
            userToDisable.UpdatedAt = DateTime.UtcNow;
            CreateUserDto user = _mapper.Map<CreateUserDto>(userToDisable);
            await _unitOfWork.UserRepository.UpdateAsync(userToDisable);
            await _unitOfWork.SaveChanges();
            return user;
        }

        public async Task<ReadUserDto> GetUserAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get user with id - {id}", id);
            User user = await _unitOfWork.UserRepository.GetAsync(id);
            return _mapper.Map<ReadUserDto>(user);
        }

        public async Task<ReadUserCommentsDto> GetUserCommentsAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get user comments with id - {id}", id);
            User user = await _unitOfWork.UserRepository.GetAsync(id);
            return _mapper.Map<ReadUserCommentsDto>(user);
        }

        public async Task<ReadUserPostLikesDto> GetUserPostLikesAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get user post likes with id - {id}", id);
            User user = await _unitOfWork.UserRepository.GetAsync(id);
            return _mapper.Map<ReadUserPostLikesDto>(user);
        }

        public async Task<ReadUserCommentLikesDto> GetUserCommentLikesAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get user comment likes with id - {id}", id);
            User user = await _unitOfWork.UserRepository.GetAsync(id);
            return _mapper.Map<ReadUserCommentLikesDto>(user);
        }

        public async Task<IEnumerable<ReadUserDto>> GetUsersAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all users");

            IEnumerable<User> users = await _unitOfWork.UserRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadUserDto>>(users);
        }

        public async Task<CreateUserDto> UpdateUserAsync(long id, CreateUserDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update user with id - {id}, name - {name}", id, item.Name);

            User userToModify = await _unitOfWork.UserRepository.GetAsync(id);

            User user = _mapper.Map(item, userToModify);
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChanges();
            return item;
        }
    }
}

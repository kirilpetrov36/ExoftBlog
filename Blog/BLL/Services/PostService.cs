using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.PostDto;
using Blog.BLL.DTO;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.Services.Interfaces;

namespace Blog.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PostService(ILogger<PostService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadPostDto> CreatePostAsync(CreatePostDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Post creation with title - {title}", item.Title);

            DateTime timeNow = DateTime.UtcNow;
            Post post = _mapper.Map<Post>(item);
            post.CreatedAt = timeNow;
            post.UpdatedAt = timeNow;
            await _unitOfWork.PostRepository.CreateAsync(post);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReadPostDto>(post);
        }

        public async Task DeletePostAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete post with id - {id}", id);
            await _unitOfWork.PostRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<CreatePostDto> ChangePostStateAsync(long id, ChangeStateDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Disable post with id - {id}", id);
            Post postToDisable = await _unitOfWork.PostRepository.GetAsync(id);
            postToDisable.IsDeleted = item.IsDeleted;
            postToDisable.UpdatedAt = DateTime.UtcNow;
            CreatePostDto post = _mapper.Map<CreatePostDto>(postToDisable);
            await _unitOfWork.PostRepository.UpdateAsync(postToDisable);
            await _unitOfWork.SaveChanges();
            return post;
        }

        public async Task<ReadPostDto> GetPostAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get post with id - {id}", id);
            Post post = await _unitOfWork.PostRepository.GetAsync(id);
            return _mapper.Map<ReadPostDto>(post);
        }

        public async Task<ReadPostCommentsDto> GetPostCommentsAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get post comments with id - {id}", id);
            Post post = await _unitOfWork.PostRepository.GetAsync(id);
            return _mapper.Map<ReadPostCommentsDto>(post);
        }

        public async Task<ReadPostLikesDto> GetPostLikesAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get post likes with id - {id}", id);
            Post post = await _unitOfWork.PostRepository.GetAsync(id);
            return _mapper.Map<ReadPostLikesDto>(post);
        }

        public async Task<IEnumerable<ReadPostDto>> GetPostsAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all posts");

            IEnumerable<Post> posts = await _unitOfWork.PostRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadPostDto>>(posts);
        }

        public async Task<IEnumerable<ReadPostDto>> GetMostCommentablePostsAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by comments amount posts");

            IEnumerable<Post> posts = await _unitOfWork.PostRepository.GetMostComentableAsync();
            return _mapper.Map<IEnumerable<ReadPostDto>>(posts);
        }

        public async Task<IEnumerable<ReadPostDto>> GetMostLikeablePostsAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by likes amount posts");

            IEnumerable<Post> posts = await _unitOfWork.PostRepository.GetMostLikeableAsync();
            return _mapper.Map<IEnumerable<ReadPostDto>>(posts);
        }

        public async Task<CreatePostDto> UpdatePostAsync(long id, CreatePostDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update post with id - {id}. New title - {title}", id, item.Title);

            Post postToModify = await _unitOfWork.PostRepository.GetAsync(id);

            Post post = _mapper.Map(item, postToModify);
            post.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PostRepository.UpdateAsync(post);
            await _unitOfWork.SaveChanges();
            return item;
        }
    }
}

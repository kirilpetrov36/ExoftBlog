using Blog.BLL.Services.Interfaces;
using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO;
using Blog.DAL.Entities;
using AutoMapper;

namespace Blog.BLL.Services
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public PostLikeService(ILogger<PostLikeService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadPostLikeDto> CreatePostLikeAsync(CreatePostLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("PostLike creation with userId - {userId} and {postId}", item.UserId, item.PostId);
            DateTime timeNow = DateTime.UtcNow;
            PostLike postLike = _mapper.Map<PostLike>(item);
            postLike.CreatedAt = timeNow;
            postLike.UpdatedAt = timeNow;
            await _unitOfWork.PostLikeRepository.CreateAsync(postLike);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReadPostLikeDto>(postLike);
        }

        public async Task DeletePostLikeAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete postLike with id - {id}", id);
            await _unitOfWork.PostLikeRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<CreatePostLikeDto> ChangePostLikeStateAsync(long id, ChangeStateDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Disable postLike with id - {id}", id);
            PostLike postLikeToDisable = await _unitOfWork.PostLikeRepository.GetAsync(id);
            postLikeToDisable.IsDeleted = item.IsDeleted;
            postLikeToDisable.UpdatedAt = DateTime.UtcNow;
            CreatePostLikeDto postLike = _mapper.Map<CreatePostLikeDto>(postLikeToDisable);
            await _unitOfWork.PostLikeRepository.UpdateAsync(postLikeToDisable);
            await _unitOfWork.SaveChanges();
            return postLike;
        }

        public async Task<ReadPostLikeDto> GetPostLikeAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get postLike with id - {id}", id);
            PostLike postLike = await _unitOfWork.PostLikeRepository.GetAsync(id);
            return _mapper.Map<ReadPostLikeDto>(postLike);
        }

        public async Task<IEnumerable<ReadPostLikeDto>> GetPostLikesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all postLikes");

            IEnumerable<PostLike> postLikes = await _unitOfWork.PostLikeRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadPostLikeDto>>(postLikes);
        }

        public async Task<CreatePostLikeDto> UpdatePostLikeAsync(long id, CreatePostLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update postLike with id - {id}.", id);

            PostLike postLikeToModify = await _unitOfWork.PostLikeRepository.GetAsync(id);

            PostLike postLike = _mapper.Map(item, postLikeToModify);
            postLike.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PostLikeRepository.UpdateAsync(postLike);
            await _unitOfWork.SaveChanges();
            return item;
        }
    }

    public class CommentLikeService : ICommentLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CommentLikeService(ILogger<CommentLikeService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadCommentLikeDto> CreateCommentLikeAsync(CreateCommentLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("CommentLike creation with userId - {userId} and {commentId}", item.UserId, item.CommentId);
            DateTime timeNow = DateTime.UtcNow;
            CommentLike commentLike = _mapper.Map<CommentLike>(item);
            commentLike.CreatedAt = timeNow;
            commentLike.UpdatedAt = timeNow;
            await _unitOfWork.CommentLikeRepository.CreateAsync(commentLike);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReadCommentLikeDto>(commentLike);
        }

        public async Task DeleteCommentLikeAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete commentLike with id - {id}", id);
            await _unitOfWork.CommentLikeRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<CreateCommentLikeDto> ChangeCommentLikeStateAsync(long id, ChangeStateDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Disable commentLike with id - {id}", id);
            CommentLike commentLikeToDisable = await _unitOfWork.CommentLikeRepository.GetAsync(id);
            commentLikeToDisable.IsDeleted = item.IsDeleted;
            commentLikeToDisable.UpdatedAt = DateTime.UtcNow;
            CreateCommentLikeDto commentLike = _mapper.Map<CreateCommentLikeDto>(commentLikeToDisable);
            await _unitOfWork.CommentLikeRepository.UpdateAsync(commentLikeToDisable);
            await _unitOfWork.SaveChanges();
            return commentLike;
        }

        public async Task<ReadCommentLikeDto> GetCommentLikeAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get commentLike with id - {id}", id);
            CommentLike commentLike = await _unitOfWork.CommentLikeRepository.GetAsync(id);
            return _mapper.Map<ReadCommentLikeDto>(commentLike);
        }

        public async Task<IEnumerable<ReadCommentLikeDto>> GetCommentLikesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all commentLikes");

            IEnumerable<CommentLike> commentLikes = await _unitOfWork.CommentLikeRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadCommentLikeDto>>(commentLikes);
        }

        public async Task<CreateCommentLikeDto> UpdateCommentLikeAsync(long id, CreateCommentLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update commentLike with id - {id}.", id);

            CommentLike commentLikeToModify = await _unitOfWork.CommentLikeRepository.GetAsync(id);

            CommentLike commentLike = _mapper.Map(item, commentLikeToModify);
            commentLike.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommentLikeRepository.UpdateAsync(commentLike);
            await _unitOfWork.SaveChanges();
            return item;
        }
    }
}

using Blog.BLL.Services.Interfaces;
using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.LikeDto;
using Blog.DAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services
{
    public class ArticleLikeService : IArticleLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public ArticleLikeService(ILogger<ArticleLikeService> logger, IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ReadArticleLikeDto> CreateArticleLikeAsync(CreateArticleLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("ArticleLike creation with articleId -  {articleId}", item.ArticleId);           
            ArticleLike articleLike = _mapper.Map<ArticleLike>(item);
            _unitOfWork.ArticleLikeRepository.CreateAsync(articleLike);
            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return _mapper.Map<ReadArticleLikeDto>(articleLike);
        }

        public async Task DeleteArticleLikeAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete articleLike with id - {id}", id);
            await _unitOfWork.ArticleLikeRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<ReadArticleLikeDto> GetArticleLikeAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get articleLike with id - {id}", id);
            ArticleLike articleLike = await _unitOfWork.ArticleLikeRepository.GetAsync(id);
            return _mapper.Map<ReadArticleLikeDto>(articleLike);
        }

        public async Task<IEnumerable<ReadArticleLikeDto>> GetArticleLikesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all articleLikes");
            IEnumerable<ArticleLike> articleLikes = await _unitOfWork.ArticleLikeRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadArticleLikeDto>>(articleLikes);
        }

        public async Task<ReadArticleLikeDto> PatchArticleLikeAsync(Guid id, JsonPatchDocument<ArticleLike> articleLikeUpdates, CancellationToken token = default)
        {
            _logger.LogInformation("Patch articleLike with id - {id}.", id);
            ArticleLike articleLikeToModify = await _unitOfWork.ArticleLikeRepository.GetAsync(id);
            articleLikeUpdates.ApplyTo(articleLikeToModify);
            _unitOfWork.ArticleLikeRepository.UpdateAsync(articleLikeToModify);
            return _mapper.Map<ReadArticleLikeDto>(articleLikeToModify);
        }

        public async Task<int?> GetArticleLikesAmountAsync(Guid ArticleId)
        {
            _logger.LogInformation("Get Article Likes Amount with ArticleId - {id}.", ArticleId);
            return await _unitOfWork.ArticleLikeRepository.GetArticleLikesAmountAsync(ArticleId);
        }
    }

    public class CommentLikeService : ICommentLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public CommentLikeService(ILogger<CommentLikeService> logger, IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ReadCommentLikeDto> CreateCommentLikeAsync(CreateCommentLikeDto item, CancellationToken token = default)
        {
            _logger.LogInformation("CommentLike creation with commentId - {commentId}",item.CommentId);
            CommentLike commentLike = _mapper.Map<CommentLike>(item);
            _unitOfWork.CommentLikeRepository.CreateAsync(commentLike);
            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return _mapper.Map<ReadCommentLikeDto>(commentLike);
        }

        public async Task DeleteCommentLikeAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete commentLike with id - {id}", id);
            await _unitOfWork.CommentLikeRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<ReadCommentLikeDto> GetCommentLikeAsync(Guid id, CancellationToken token = default)
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

        public async Task<ReadCommentLikeDto> PatchCommentLikeAsync(Guid id, JsonPatchDocument<CommentLike> commentLikeUpdates, CancellationToken token = default)
        {
            _logger.LogInformation("Patch commentLike with id - {id}.", id);

            CommentLike commentLikeToModify = await _unitOfWork.CommentLikeRepository.GetAsync(id);
            commentLikeUpdates.ApplyTo(commentLikeToModify);
            _unitOfWork.CommentLikeRepository.UpdateAsync(commentLikeToModify);
            return _mapper.Map<ReadCommentLikeDto>(commentLikeToModify);
        }

        public async Task<int?> GetCommentLikesAmountAsync(Guid CommentId)
        {
            _logger.LogInformation("Get Comment Likes Amount with CommentId - {id}.", CommentId);
            return await _unitOfWork.CommentLikeRepository.GetCommentLikesAmountAsync(CommentId);
        }
    }
}

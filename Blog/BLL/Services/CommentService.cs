using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.CommentDto;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.Services.Interfaces;
using Blog.BLL.DTO.ArticleDto;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public CommentService(ILogger<CommentService> logger, IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ReadCommentDto> CreateCommentAsync(CreateCommentDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Comment creation with text - {title}", item.Text);

            Comment comment = _mapper.Map<Comment>(item);
            _unitOfWork.CommentRepository.CreateAsync(comment);
            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return _mapper.Map<ReadCommentDto>(comment);
        }

        public async Task DeleteCommentAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete comment with id - {id}", id);
            await _unitOfWork.CommentRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<ReadCommentDto> GetCommentAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get comment with id - {id}", id);
            Comment comment = await _unitOfWork.CommentRepository.GetAsync(id);
            return _mapper.Map<ReadCommentDto>(comment);
        }

        public async Task<ReadCommentLikesDto> GetLikesAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get comment likes with id - {id}", id);
            Comment comment = await _unitOfWork.CommentRepository.GetAsync(id);
            return _mapper.Map<ReadCommentLikesDto>(comment);
        }

        public async Task<ReadCommentChildsDto> GetCommentChildsAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get comment childs with id - {id}", id);
            Comment comment = await _unitOfWork.CommentRepository.GetAsync(id);
            return _mapper.Map<ReadCommentChildsDto>(comment);
        }

        public async Task<IEnumerable<ReadCommentDto>> GetCommentsAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all comments");
            IEnumerable<Comment> comments = await _unitOfWork.CommentRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadCommentDto>>(comments);
        }

        public async Task<CreateCommentDto> UpdateCommentAsync(Guid id, CreateCommentDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update comment with id - {id}.", id);

            Comment commentToModify = await _unitOfWork.CommentRepository.GetAsync(id);
            Comment comment = _mapper.Map(item, commentToModify);

            _unitOfWork.CommentRepository.UpdateAsync(comment);
            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return item;
        }

        public async Task<ReadCommentDto> PatchCommentAsync(Guid id, JsonPatchDocument<Comment> commentUpdates, CancellationToken token = default)
        {
            _logger.LogInformation("Patch comment with id - {id}.", id);

            Comment commentToModify = await _unitOfWork.CommentRepository.GetAsync(id);
            commentUpdates.ApplyTo(commentToModify);
            _unitOfWork.CommentRepository.UpdateAsync(commentToModify);
            return _mapper.Map<ReadCommentDto>(commentToModify);
        }

        public async Task<int?> GetArticleCommentsAmount(Guid ArticleId)
        {
            return await _unitOfWork.CommentRepository.GetArticleCommentsAmount(ArticleId);
        }
    }
}

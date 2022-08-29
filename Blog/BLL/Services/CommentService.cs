using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.Services.Interfaces;

namespace Blog.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CommentService(ILogger<CommentService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ReadCommentDto> CreateCommentAsync(CreateCommentDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Comment creation with text - {title}", item.Text);

            DateTime timeNow = DateTime.UtcNow;
            Comment comment = _mapper.Map<Comment>(item);
            comment.CreatedAt = timeNow;
            comment.UpdatedAt = timeNow;
            await _unitOfWork.CommentRepository.CreateAsync(comment);
            await _unitOfWork.SaveChanges();
            return _mapper.Map<ReadCommentDto>(comment);
        }

        public async Task DeleteCommentAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete comment with id - {id}", id);
            await _unitOfWork.CommentRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<CreateCommentDto> ChangeCommentStateAsync(long id, ChangeStateDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Disable comment with id - {id}", id);
            Comment commentToDisable = await _unitOfWork.CommentRepository.GetAsync(id);
            commentToDisable.IsDeleted = item.IsDeleted;
            commentToDisable.UpdatedAt = DateTime.UtcNow;
            CreateCommentDto comment = _mapper.Map<CreateCommentDto>(commentToDisable);
            await _unitOfWork.CommentRepository.UpdateAsync(commentToDisable);
            await _unitOfWork.SaveChanges();
            return comment;
        }

        public async Task<ReadCommentDto> GetCommentAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get comment with id - {id}", id);
            Comment comment = await _unitOfWork.CommentRepository.GetAsync(id);
            return _mapper.Map<ReadCommentDto>(comment);
        }

        public async Task<ReadCommentLikesDto> GetLikesAsync(long id, CancellationToken token = default)
        {
            _logger.LogInformation("Get comment likes with id - {id}", id);
            Comment comment = await _unitOfWork.CommentRepository.GetAsync(id);
            return _mapper.Map<ReadCommentLikesDto>(comment);
        }

        public async Task<ReadCommentChildsDto> GetCommentChildsAsync(long id, CancellationToken token = default)
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

        public async Task<CreateCommentDto> UpdateCommentAsync(long id, CreateCommentDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update comment with id - {id}.", id);

            Comment commentToModify = await _unitOfWork.CommentRepository.GetAsync(id);

            Comment comment = _mapper.Map(item, commentToModify);
            comment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommentRepository.UpdateAsync(comment);
            await _unitOfWork.SaveChanges();
            return item;
        }
    }
}

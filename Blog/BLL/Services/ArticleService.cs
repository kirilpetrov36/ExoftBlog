using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public ArticleService(ILogger<ArticleService> logger, IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ReadArticleDto> CreateArticleAsync(CreateArticleDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Article creation with title - {title}", item.Title);

            Article article = _mapper.Map<Article>(item);
            _unitOfWork.ArticleRepository.CreateAsync(article);

            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return _mapper.Map<ReadArticleDto>(article);
        }

        public async Task DeleteArticleAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Delete article with id - {id}", id);
            await _unitOfWork.ArticleRepository.DeleteAsync(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<ReadArticleDto> GetArticleAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get article with id - {id}", id);
            Article article = await _unitOfWork.ArticleRepository.GetAsync(id);
            return _mapper.Map<ReadArticleDto>(article);
        }

        public async Task<ReadArticleCommentsDto> GetArticleCommentsAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get article comments with id - {id}", id);
            Article article = await _unitOfWork.ArticleRepository.GetAsync(id);
            return _mapper.Map<ReadArticleCommentsDto>(article);
        }

        public async Task<ReadArticleLikesDto> GetArticleLikesAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get article likes with id - {id}", id);
            Article article = await _unitOfWork.ArticleRepository.GetAsync(id);
            return _mapper.Map<ReadArticleLikesDto>(article);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all articles");

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetListAsync();
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetMostCommentableArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by comments amount articles");

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetMostComentableAsync();
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetMostLikeableArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by likes amount articles");

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetMostLikeableAsync();
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<ReadArticleDto> UpdateArticleAsync(Guid id, CreateArticleDto item, CancellationToken token = default)
        {
            _logger.LogInformation("Update article with id - {id}.", id);

            Article articleToModify = await _unitOfWork.ArticleRepository.GetAsync(id);
            Article article = _mapper.Map(item, articleToModify);

            _unitOfWork.ArticleRepository.UpdateAsync(article);
            await _unitOfWork.SaveChanges(_accountService.GetUserId());
            return _mapper.Map<ReadArticleDto>(article); 
        }

        public async Task<ReadArticleDto> PatchArticleAsync(Guid id, JsonPatchDocument<Article> articleUpdates, CancellationToken token = default)
        {
            _logger.LogInformation("Patch article with id - {id}.", id);

            Article articleToModify = await _unitOfWork.ArticleRepository.GetAsync(id);
            articleUpdates.ApplyTo(articleToModify);
            _unitOfWork.ArticleRepository.UpdateAsync(articleToModify);
            return _mapper.Map<ReadArticleDto>(articleToModify);
        }
    }
}


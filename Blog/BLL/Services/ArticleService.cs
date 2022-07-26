﻿using Blog.DAL.UnitOfWork;
using Blog.BLL.DTO.ArticleDto;
using Blog.DAL.Entities;
using AutoMapper;
using Blog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Blog.BLL.DTO.ArticleFileDto;
using Blog.BLL.DTO.CommentDto;
using Blog.BLL.DTO.LikeDto;
using Blog.BLL.DTO.UserDto;

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
            if (article?.Likes != null)
            {
                article.LikesAmount = article.Likes.Count();
            }
            if (article?.Comments != null)
            {
                article.CommentsAmount = article.Comments.Count();
            }    
            return _mapper.Map<ReadArticleDto>(article);
        }

        public async Task<ReadFullArticleDto> GetFullArticleAsync(Guid id, CancellationToken token = default)
        {
            _logger.LogInformation("Get full article with id - {id}", id);
            Article article = await _unitOfWork.ArticleRepository.GetAsync(id);
            ReadFullArticleDto fullArticleDto = new ReadFullArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Likes = _mapper.Map<List<ReadArticleLikeDto>>(article.Likes),       // we need ReadArticleLikeDto not ArticleLike to prevent loop
                Comments = _mapper.Map<List<ReadCommentDto>>(article.Comments),     // Article => ArticleLikes => ArticleLike => Article
                ArticleFiles = _mapper.Map<List<ReadArticleFileDto>>(article.ArticleFiles),
                IsDeleted = article.IsDeleted,
                IsVerified = article.IsVerified,
                User = _mapper.Map<ReadUserDto>(article.User),
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
                CreatedBy = article.CreatedBy,
                UpdatedBy = article.UpdatedBy
            };

            return fullArticleDto;
        }

        public async Task<IEnumerable<ReadArticleDto>> SearchArticles(string searchInput, CancellationToken token = default)
        {
            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.SearchArticles(searchInput);
            foreach (Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
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
            foreach(Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetMostCommentableArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by comments amount articles");

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetMostComentableAsync();
            foreach (Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetMostLikeableArticlesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Get all sorted by likes amount articles");

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetMostLikeableAsync();
            foreach (Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetArticlesBySubscriptionAsync(CancellationToken token = default)
        {
            Guid currentUserId = _accountService.GetUserId();
            _logger.LogInformation("Get articles for which current user {currentUserId} subscribed", currentUserId);

            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetArticlesBySubscriptionAsync(currentUserId);
            foreach (Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
            return _mapper.Map<IEnumerable<ReadArticleDto>>(articles);
        }

        public async Task<IEnumerable<ReadArticleDto>> GetUserArticlesAsync(Guid userId, CancellationToken token = default)
        {
            _logger.LogInformation("Get articles for user {UserId} subscribed", userId);
            IEnumerable<Article> articles = await _unitOfWork.ArticleRepository.GetUserArticlesAsync(userId);
            foreach (Article article in articles)
            {
                article.LikesAmount = await _unitOfWork.ArticleRepository.GetLikesAmount(article.Id);
                article.CommentsAmount = await _unitOfWork.ArticleRepository.GetCommentsAmount(article.Id);
            }
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


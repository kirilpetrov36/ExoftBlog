using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;
using Blog.DAL.UnitOfWork;

namespace Blog.BLL.Services.ExternalServices
{
    public class ArticleFileService : IArticleFileService
    {
        public IConfiguration Configuration { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public ArticleFileService(IConfiguration configuration, IUnitOfWork unitOfWork, IAccountService accountService, ILogger logger)
        {
            Configuration = configuration;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task<List<ArticleFile>> UploadFilesAsync(ICollection<IFormFile> files, Guid postId)
        {   
            try
            {
                if (files.Count == 0)
                    return null;

                var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
                );

                List<ArticleFile> dataFiles = new List<ArticleFile>();


                foreach (var formFile in files)
                {
                    var PostId = postId.ToString();
                    var fileName = formFile.FileName;
                    string guid = Guid.NewGuid().ToString();
                    var client = container.GetBlobClient($"/Posts/{PostId}/{guid}_{fileName}");

                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        {
                            var fileBytes = ReadFully(stream);
                            var data = new BinaryData(fileBytes);
                            await client.UploadAsync(data);
                        }
                        ArticleFile file = new ArticleFile()
                        {
                            Url = container.Uri + $"/Posts/{PostId}/{guid}_{fileName}",
                            BlobName = $"/Posts/{PostId}/{guid}_{fileName}",
                            ArticleId = postId
                        };
                        dataFiles.Add(file);
                        _unitOfWork.ArticleFileRepository.CreateAsync(file);
                        await _unitOfWork.SaveChanges(_accountService.GetUserId());
                        
                    }
                }
                return dataFiles;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }

        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public async Task<bool> RemoveFileAsync(Guid FileId)
        {
            var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
            );
            ArticleFile file = await _unitOfWork.ArticleFileRepository.GetAsync(FileId);
            if (file == null)
            {
                _logger.LogInformation($"File with id {FileId} wasn't found");
                return false;
            }

            await container.DeleteBlobAsync(file.BlobName, DeleteSnapshotsOption.None);               
            _unitOfWork.ArticleFileRepository.DeleteAsync(file);
            await _unitOfWork.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<List<ArticleFile>> GetPostImages(Guid postId)
        {
            List<ArticleFile> images = new List<ArticleFile>();
            var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
            );
            var PostId = postId.ToString();
            var blobHierarchyItems = container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", prefix: $"Posts/{PostId}/");

            await foreach (var blobHierarchyItem in blobHierarchyItems)
            {                
                string blobName = blobHierarchyItem.Blob.Name;
                images.Add(new ArticleFile() { 
                    BlobName = blobName, 
                    Url = container.Uri + "/" + blobName 
                });
            }
            
            return await Task.FromResult(images);
        }

        public async Task<ArticleFile> GetFileById(Guid FileId)
        {
            ArticleFile file = await _unitOfWork.ArticleFileRepository.GetAsync(FileId);
            return file;
        }
    }
    
}


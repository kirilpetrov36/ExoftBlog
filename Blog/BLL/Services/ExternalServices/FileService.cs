using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.Entities;
using Blog.DAL.UnitOfWork;

namespace Blog.BLL.Services.ExternalServices
{
    public class FileService : IFileService
    {
        public IConfiguration Configuration { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public FileService(IConfiguration configuration, IUnitOfWork unitOfWork, IAccountService accountService, ILogger logger)
        {
            Configuration = configuration;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task<List<DataFile>> UploadFilesAsync(ICollection<IFormFile> files, Guid postId)
        {   
            try
            {
                if (files.Count == 0)
                    return null;

                var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString") ,
                    Configuration.GetConnectionString("blobStorageContainerName")
                );

                List<DataFile> dataFiles = new List<DataFile>();


                foreach (var formFile in files)
                {
                    var PostId = postId.ToString();
                    var fileName = formFile.FileName;
                    var client = container.GetBlobClient($"/Posts/{PostId}/{fileName}");

                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        {
                            var fileBytes = ReadFully(stream);
                            var data = new BinaryData(fileBytes);
                            await client.UploadAsync(data);
                        }
                        DataFile file = new DataFile()
                        {
                            Url = container.Uri + $"/Posts/{PostId}/{fileName}",
                            BlobName = $"/Posts/{PostId}/{fileName}",
                            ArticleId = postId
                        };
                        dataFiles.Add(file);
                        _unitOfWork.DataFileRepository.CreateAsync(file);
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
            DataFile file = await _unitOfWork.DataFileRepository.GetAsync(FileId);
            if (file == null)
            {
                _logger.LogInformation($"File with id {FileId} wasn't found");
                return false;
            }

            await container.DeleteBlobAsync(file.BlobName, DeleteSnapshotsOption.None);               
            _unitOfWork.DataFileRepository.DeleteAsync(file);
            await _unitOfWork.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<List<DataFile>> GetPostImages(Guid postId)
        {
            List<DataFile> images = new List<DataFile>();
            var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
            );
            var PostId = postId.ToString();
            var blobHierarchyItems = container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", prefix: $"Posts/{PostId}/");

            await foreach (var blobHierarchyItem in blobHierarchyItems)
            {                
                string blobName = blobHierarchyItem.Blob.Name;
                images.Add(new DataFile() { 
                    BlobName = blobName, 
                    Url = container.Uri + "/" + blobName 
                });
            }
            
            return await Task.FromResult(images);
        }

        public async Task<DataFile> GetFileById(Guid FileId)
        {
            DataFile file = await _unitOfWork.DataFileRepository.GetAsync(FileId);
            return file;
        }
    }
    
}


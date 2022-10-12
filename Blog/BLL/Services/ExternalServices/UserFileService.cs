using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Blog.BLL.Services.Interfaces;
using Blog.DAL.UnitOfWork;
using Blog.DAL.Entities;

namespace Blog.BLL.Services.ExternalServices
{
    public class UserFileService : IUserFileService
    {
        public IConfiguration Configuration { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public UserFileService(IConfiguration configuration, IUnitOfWork unitOfWork, IAccountService accountService, ILogger logger)
        {
            Configuration = configuration;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task<List<UserFile>> UploadFilesAsync(ICollection<IFormFile> files)
        {
            try
            {
                if (files.Count == 0)
                    return null;

                var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
                );

                List<UserFile> dataFiles = new List<UserFile>();


                foreach (var formFile in files)
                {
                    var userId = _accountService.GetUserId().ToString();
                    var fileName = formFile.FileName;
                    string guid = Guid.NewGuid().ToString();
                    var client = container.GetBlobClient($"/Users/{userId}/{guid}_{fileName}");

                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        {
                            var fileBytes = ReadFully(stream);
                            var data = new BinaryData(fileBytes);
                            await client.UploadAsync(data);
                        }
                        UserFile file = new UserFile()
                        {
                            Url = container.Uri + $"/Users/{userId}/{guid}_{fileName}",
                            BlobName = $"/Users/{userId}/{guid}_{fileName}",
                            UserId = Guid.Parse(userId)
                        };
                        dataFiles.Add(file);
                        _unitOfWork.UserFileRepository.CreateAsync(file);
                        await _unitOfWork.SaveChanges(Guid.Parse(userId));

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
            UserFile file = await _unitOfWork.UserFileRepository.GetAsync(FileId);
            if (file == null)
            {
                _logger.LogInformation($"File with id {FileId} wasn't found");
                return false;
            }

            await container.DeleteBlobAsync(file.BlobName, DeleteSnapshotsOption.None);
            _unitOfWork.UserFileRepository.DeleteAsync(file);
            await _unitOfWork.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<List<UserFile>> GetFiles()
        {
            List<UserFile> files = new List<UserFile>();
            var container = new BlobContainerClient(
                    Configuration.GetConnectionString("blobStorageConnectionString"),
                    Configuration.GetConnectionString("blobStorageContainerName")
            );
            var UserId = _accountService.GetUserId().ToString();
            var blobHierarchyItems = container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", prefix: $"Users/{UserId}/");

            await foreach (var blobHierarchyItem in blobHierarchyItems)
            {
                string blobName = blobHierarchyItem.Blob.Name;
                files.Add(new UserFile()
                {
                    BlobName = blobName,
                    Url = container.Uri + "/" + blobName
                });
            }

            return await Task.FromResult(files);
        }

        public async Task<UserFile> GetFileById(Guid FileId)
        {
            UserFile file = await _unitOfWork.UserFileRepository.GetAsync(FileId);
            return file;
        }
    }
}

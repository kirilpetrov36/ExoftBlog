using Blog.BLL.DTO.File;
using Blog.DAL.Entities;
using Blog.DAL.Enums;
using Blog.BLL.Constants;
using Blog.DAL.UnitOfWork;
using AutoMapper;

namespace Blog.BLL.Services
{
    public class FileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReadFileDto> InsertFileAsync(IFormFile formFile)
        {
            string fileName = formFile.FileName;
            var path = Path.Combine("wwwroot/files", fileName);

            var stream = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(stream);

            string url = "/files/" + fileName;

            MediaFile mediaFile = new MediaFile();
            mediaFile.FilePath = url;
            mediaFile.AltName = null; //need to implement

            ContentType content;
            if (Consts.SupportedFileFormats.TryGetValue(formFile.ContentType, out content))
            {
                mediaFile.ContentType = content;
            }
            else
            {
                throw new ArgumentException(ErrorMessages.InvalidFileFormat);
            }

            await _unitOfWork.MediaFileRepository.CreateAsync(mediaFile);
            return _mapper.Map<ReadFileDto>(mediaFile);

        }
    }
}

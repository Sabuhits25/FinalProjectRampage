using Microsoft.AspNetCore.Http;
using Rampage.BLL.Services.Interfaces;

namespace Rampage.BLL.Services.Abstraction
{
    public class FileManagerService : IFileManagerService
    {
        private readonly ICloudinaryService _cloudinaryService;

        public FileManagerService(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public bool BeAValidImage(IFormFile file)
        {
            return file != null && file.ContentType.Contains("image") && 1024 * 1024 * 5 >= file.Length;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("The file parameter cannot be null.");

            if (!BeAValidImage(file))
                throw new Exception("The file format is not valid. You have to upload an image type file and it should be a maximum of 5MB.");

            return await _cloudinaryService.UploadImageAsync(file);
        }
    }
}

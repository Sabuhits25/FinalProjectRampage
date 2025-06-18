using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Rampage.BLL.Services.Interfaces;

namespace Rampage.BLL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "uploads",
                    PublicId = Guid.NewGuid().ToString() + "_" + Path.GetFileNameWithoutExtension(file.FileName),
                    Overwrite = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception("Image upload failed: " + uploadResult.Error?.Message);

                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}

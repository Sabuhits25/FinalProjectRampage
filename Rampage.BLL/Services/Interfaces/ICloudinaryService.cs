using Microsoft.AspNetCore.Http;

namespace Rampage.BLL.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}

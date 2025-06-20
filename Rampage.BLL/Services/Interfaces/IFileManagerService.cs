using Microsoft.AspNetCore.Http;

namespace Rampage.BLL.Services.Interfaces
{
    public interface IFileManagerService
    {
        bool BeAValidImage(IFormFile file);
        Task<string> UploadFileAsync(IFormFile file);
    }
}

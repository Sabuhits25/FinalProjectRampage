using Microsoft.AspNetCore.Http;

namespace Rampage.BLL.DTO_s.Commons
{
    public interface IAuditedEntityDTO
    {
        public IFormFile Image { get; set; }
    }
}

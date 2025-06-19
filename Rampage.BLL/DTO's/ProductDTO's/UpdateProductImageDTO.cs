using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class UpdateProductImageDTO : BaseEntityDTO, IAuditedEntityDTO
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
    }
}

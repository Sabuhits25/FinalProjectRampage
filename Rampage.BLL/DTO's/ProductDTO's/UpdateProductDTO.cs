using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class UpdateProductDTO : BaseEntityDTO
    {
        public string Code { get; set; }
        public string BarCode { get; set; }
        public float Star { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int PageIndex { get; set; }

        public ICollection<int> ColorIds { get; set; }
        public ICollection<CurrentProductImageDTO>? CurrentImages { get; set; }
        public string? RemovedProductImageIds { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public List<UpdateProductTranslationDTO> Translations { get; set; }
        public List<UpdateProductSettingDTO> Settings { get; set; }
    }
}

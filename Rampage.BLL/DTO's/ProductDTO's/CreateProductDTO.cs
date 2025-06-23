using Microsoft.AspNetCore.Http;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class CreateProductDTO
    {
        public string Code { get; set; }
        public string BarCode { get; set; }
        public float Star { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public ICollection<int> ColorIds { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public ICollection<CreateProductTranslationDTO> Translations { get; set; }
        public ICollection<CreateProductSettingDTO> Settings { get; set; }
    }
}

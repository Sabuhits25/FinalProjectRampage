using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class ProductDTO : BaseEntityDTO
    {
        public float? Star { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? BarCode { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public List<string>? ImageUrls { get; set; }
        public Dictionary<string, string>? Settings { get; set; }
        public string? CategoryImageUrl { get; set; }
    }
}

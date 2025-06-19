using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.Core.Models;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class FilterProductDTO
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<CategoryDTO> Categories { get; set; }
        public ICollection<ColorDTO> Colors { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }

        public int CategoryId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<int> ColorIds { get; set; }
    }
}

using Rampage.Core.Entities;

namespace Rampage.Core.Models
{
    public class Product
    {
        public string Code { get; set; }
        public string BarCode { get; set; }
        public float Star { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductColor>? Colors { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ProductImage>? Images { get; set; }
        public ICollection<ProductSetting>? Settings { get; set; }
        public ICollection<ProductTranslation>? Translations { get; set; }
    }
}

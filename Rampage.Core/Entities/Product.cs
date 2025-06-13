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
    }
}

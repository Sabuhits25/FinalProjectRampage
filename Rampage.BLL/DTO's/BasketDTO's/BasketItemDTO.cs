namespace Rampage.BLL.DTO_s.BasketDTO_s
{
    public class BasketItemDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public bool? IncreaseOrDecrease { get; set; }
    }
}

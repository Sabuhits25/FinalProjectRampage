namespace Rampage.BLL.DTO_s.BasketDTO_s
{
    public class BasketDTO
    {
        public ICollection<BasketItemDTO>? BasketItems { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public decimal ProductsPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? UserId { get; set; }
        public bool IsGuest { get; set; }
    }
}

namespace Rampage.BLL.DTO_s.TransactionDTO_s
{
    public class CreateTransactionDTO
    {
        public decimal ProductsPrice { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public decimal Amount { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Lang { get; set; }
        public string? City { get; set; }
    }
}

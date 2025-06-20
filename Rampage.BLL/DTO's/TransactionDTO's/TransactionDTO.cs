using Rampage.Core.Entities.Identity;

namespace Rampage.BLL.DTO_s.TransactionDTO_s
{
    public class TransactionDTO
    {
        public User User { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string SessionId { get; set; }
        public string CheckToken { get; set; }
        public string Status { get; set; }
        public string ResponseBody { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public int PageIndex { get; set; }
    }
}

using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;

namespace Rampage.Core.Entities
{
    public class Transaction : BaseEntity, IAuditedEntity
    {
        public User? User { get; set; }
        public string OrderId { get; set; }
        public string? UserId { get; set; }
        public decimal Amount { get; set; }
        public string SessionId { get; set; }
        public string CheckToken { get; set; }
        public string ResponseBody { get; set; }

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

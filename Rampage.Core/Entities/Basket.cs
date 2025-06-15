using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;

namespace Rampage.Core.Entities
{
    public class Basket : BaseEntity, IAuditedEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

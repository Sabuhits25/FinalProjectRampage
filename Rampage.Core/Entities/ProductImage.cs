using Rampage.Core.Entities.Commons;
using Rampage.Core.Models;

namespace Rampage.Core.Entities
{
    public class ProductImage : BaseEntity, IAuditedEntity
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

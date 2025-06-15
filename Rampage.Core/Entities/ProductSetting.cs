using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.Core.Models;

namespace Rampage.Core.Entities
{
    public class ProductSetting : BaseEntity, IAuditedEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public ELanguage Language { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

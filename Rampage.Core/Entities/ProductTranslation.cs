using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;

namespace Rampage.Core.Entities
{
    public class ProductTranslation : BaseEntity, IAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ELanguage Language { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

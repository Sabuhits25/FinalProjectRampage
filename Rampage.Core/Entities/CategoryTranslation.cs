using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.Core.Models;

namespace Rampage.Core.Entities
{
    public class CategoryTranslation : BaseEntity, IAuditedEntity
    {
        public string Name { get; set; }
        public ELanguage Language { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

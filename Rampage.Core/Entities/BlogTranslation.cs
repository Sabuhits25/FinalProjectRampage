using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;

namespace Rampage.Core.Entities
{
    public class BlogTranslation : BaseEntity, IAuditedEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ELanguage Language { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

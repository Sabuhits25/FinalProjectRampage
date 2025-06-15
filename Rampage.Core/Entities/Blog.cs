using Rampage.Core.Entities.Commons;

namespace Rampage.Core.Entities
{
    public class Blog : BaseEntity, IAuditedEntity
    {
        public string Author { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<BlogTranslation>? Translations { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }


}

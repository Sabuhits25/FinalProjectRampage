using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;

namespace Rampage.Core.Entities
{
    public class ColorTranslation : BaseEntity, IAuditedEntity
    {
        public string Name { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public ELanguage Language { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.Core.Models
{
    public class Category : BaseEntity, IAuditedEntity
    {
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CategoryTranslation>? Translations { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

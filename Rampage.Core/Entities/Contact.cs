using Rampage.Core.Entities.Commons;

namespace Rampage.Core.Entities
{
    public class Contact : BaseEntity, IAuditedEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using Rampage.Core.Entities.Commons;

namespace Rampage.Core.Entities
{
    public class Setting : BaseEntity, IAuditedEntity
    {
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public int DailyUsers { get; set; }
        public byte DailyProducts { get; set; }
        public byte CompanyCount { get; set; }
        public int CustomerCount { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

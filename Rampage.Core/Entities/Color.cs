﻿using Rampage.Core.Entities.Commons;

namespace Rampage.Core.Entities
{
    public class Color : BaseEntity, IAuditedEntity
    {
        public ICollection<ProductColor>? Products { get; set; }
        public ICollection<ColorTranslation>? Translations { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

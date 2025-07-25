﻿using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;

namespace Rampage.Core.Entities
{
    public class Comment : BaseEntity, IAuditedEntity
    {
        public float Star { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public ELanguage Language { get; set; }
        public string ImageUrl { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

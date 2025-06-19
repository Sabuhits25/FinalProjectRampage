using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Models;

namespace Rampage.BLL.DTO_s.CategoryDTO_s
{
    public class CreateCategoryDTO : IAuditedEntityDTO
    {
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<CreateCategoryTranslationDTO>? Translations { get; set; }
        public IFormFile Image { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.CategoryDTO_s
{
    public class UpdateCategoryDTO : BaseEntityDTO, IAuditedEntityDTO
    {
        public int? ParentCategoryId { get; set; }
        public List<UpdateCategoryTranslationDTO>? Translations { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public int PageIndex { get; set; }
    }
}

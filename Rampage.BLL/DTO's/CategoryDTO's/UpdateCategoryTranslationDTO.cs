using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.CategoryDTO_s
{
    public class UpdateCategoryTranslationDTO : BaseEntityDTO
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public ELanguage Language { get; set; }
    }
}

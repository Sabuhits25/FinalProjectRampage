using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.BlogDTO_s
{
    public class UpdateBlogTranslationDTO : BaseEntityDTO
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ELanguage Language { get; set; }
    }
}

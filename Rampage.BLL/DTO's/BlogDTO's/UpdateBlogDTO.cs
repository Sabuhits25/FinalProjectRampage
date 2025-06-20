using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.BlogDTO_s
{
    public class UpdateBlogDTO : BaseEntityDTO, IAuditedEntityDTO
    {
        public string Author { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public int PageIndex { get; set; }

        public List<UpdateBlogTranslationDTO> Translations { get; set; }
    }
}

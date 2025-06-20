using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.BlogDTO_s
{
    public class CreateBlogDTO : IAuditedEntityDTO
    {
        public string Author { get; set; }
        public IFormFile Image { get; set; }

        public List<CreateBlogTranslationDTO>? Translations { get; set; }
    }
}

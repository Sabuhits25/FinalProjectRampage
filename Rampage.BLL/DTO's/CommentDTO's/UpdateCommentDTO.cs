using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.CommentDTO_s
{
    public class UpdateCommentDTO : BaseEntityDTO, IAuditedEntityDTO
    {
        public float Star { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public ELanguage Language { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        public int PageIndex { get; set; }
    }
}

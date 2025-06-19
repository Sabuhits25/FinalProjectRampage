using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.ColorDTO_s
{
    public class UpdateColorTranslationDTO : BaseEntityDTO
    {
        public int ColorId { get; set; }
        public string Name { get; set; }
        public ELanguage Language { get; set; }
    }
}

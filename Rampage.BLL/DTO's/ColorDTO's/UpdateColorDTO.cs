using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.ColorDTO_s
{
    public class UpdateColorDTO : BaseEntityDTO
    {
        public List<UpdateColorTranslationDTO>? Translations { get; set; }
        public int PageIndex { get; set; }
    }
}

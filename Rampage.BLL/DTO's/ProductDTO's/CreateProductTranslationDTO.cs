using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class CreateProductTranslationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public ELanguage Language { get; set; }
    }
}

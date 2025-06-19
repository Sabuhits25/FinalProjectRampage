using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class CreateProductSettingDTO
    {
        public int ProductId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public ELanguage Language { get; set; }
    }
}

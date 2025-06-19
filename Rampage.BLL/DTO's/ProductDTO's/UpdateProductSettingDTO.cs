using Rampage.BLL.DTO_s.Commons;
using Rampage.Core.Enums;

namespace Rampage.BLL.DTO_s.ProductDTO_s
{
    public class UpdateProductSettingDTO : BaseEntityDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public ELanguage Language { get; set; }
        public int ProductId { get; set; }
    }
}

using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.CategoryDTO_s
{
    public class CategoryDTO : BaseEntityDTO
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}

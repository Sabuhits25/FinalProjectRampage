using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryDTO>> GetAllCategoriesByTranslationAsync(string lang);
        Task<ICollection<CategoryDTO>> GetAllNotDeletedCategoriesAsync();
        Task<PaginatedQueryable<Category>> GetAllAsync(
             bool statement, int pageIndex, int pageSize);
        Task<Category> GetByIdAsync(int id);
        Task CreateAsync(CreateCategoryDTO dto);
        Task UpdateAsync(UpdateCategoryDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

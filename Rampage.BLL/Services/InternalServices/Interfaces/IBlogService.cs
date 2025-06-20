using Rampage.BLL.DTO_s.BlogDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface IBlogService
    {
        Task<ICollection<BlogDTO>> GetAllBlogsByTranslationAsync(string lang);
        Task<PaginatedQueryable<Blog>> GetAllAsync(
            bool statement, int pageIndex, int pageSize);
        Task<Blog> GetByIdAsync(int id);
        Task<Blog> CreateAsync(CreateBlogDTO dto);
        Task<Blog> UpdateAsync(UpdateBlogDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

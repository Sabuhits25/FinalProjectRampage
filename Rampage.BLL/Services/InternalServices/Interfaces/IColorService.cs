using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface IColorService
    {
        Task<ICollection<ColorDTO>> GetAllColorsByTranslationAsync(string lang);
        Task<ICollection<ColorDTO>> GetAllNotDeletedColorsAsync();
        Task<PaginatedQueryable<Color>> GetAllAsync(
             bool statement, int pageIndex, int pageSize);
        Task<Color> GetByIdAsync(int id);
        Task CreateAsync(CreateColorDTO dto);
        Task UpdateAsync(UpdateColorDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

using Rampage.BLL.DTO_s.ContactDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface IContactService
    {
        Task<PaginatedQueryable<Contact>> GetAllAsync(
            bool statement, int pageIndex, int pageSize);
        Task<Contact> GetByIdAsync(int id);
        Task CreateAsync(CreateContactDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

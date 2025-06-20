using Rampage.BLL.DTO_s.SubscriptionDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface ISubscriptionService
    {
        Task<PaginatedQueryable<Subscription>> GetAllAsync(
            bool statement, int pageIndex, int pageSize);
        Task<bool> CheckSubscriptionAsync(string email);
        Task<Subscription> GetByIdAsync(int id);
        Task<Subscription> CreateAsync(CreateSubscriptionDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

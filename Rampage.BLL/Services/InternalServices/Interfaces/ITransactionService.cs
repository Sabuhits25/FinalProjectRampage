using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface ITransactionService
    {
        Task<PaginatedQueryable<Transaction>> GetAllAsync(
            bool statement, string query, string status,
            int pageIndex, int pageSize);
        Task<Transaction> GetByIdAsync(int id);
        Task CreateAsync(CreateTransactionDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

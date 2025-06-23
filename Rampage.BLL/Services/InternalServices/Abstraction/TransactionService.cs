using AutoMapper;
using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;
namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class TransactionService : ITransactionService
    {
        protected readonly ITransactionRepository _transactionRepository;
        protected readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateTransactionDTO dto)
        {
            var entity = _mapper.Map<Transaction>(dto);
            await _transactionRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _transactionRepository.DeleteAsync(
                await _transactionRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Transaction>> GetAllAsync(
            bool statement, string query, string status,
            int pageIndex, int pageSize)
        {
            var entities = await _transactionRepository.GetAllAsync(x => statement ? true : !x.IsDeleted,
                u => u.User);

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<EOrderStatus>(status, out var parsedStatus))
            {
                entities = entities.Where(t => t.Status == parsedStatus).ToList();
            }

            if (!string.IsNullOrEmpty(query))
            {
                entities = entities.Where(t =>
                    t.User.UserName.ToLower().Contains(query.ToLower()) ||
                    t.User.Email.ToLower().Contains(query.ToLower()) ||
                    ($"{t.User.FirstName} {t.User.LastName}" != null &&
                    $"{t.User.FirstName} {t.User.LastName}"
                    .ToLower().Contains(query.ToLower())) ||
                    t.OrderId.ToString().Contains(query)
                ).ToList();
            }

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Transaction>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdAsync(x => x.Id == id,
                u => u.User);
        }

        public async Task RecoverAsync(int id)
        {
            await _transactionRepository.RecoverAsync(
                await _transactionRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _transactionRepository.RemoveAsync(
                await _transactionRepository.GetByIdAsync(x => x.Id == id));
        }
    }
}

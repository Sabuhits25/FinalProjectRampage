using AutoMapper;
using Rampage.BLL.DTO_s.SubscriptionDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class SubscriptionService : ISubscriptionService
    {
        protected readonly ISubscriptionRepository _subscriptionRepository;
        protected readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<Subscription> CreateAsync(CreateSubscriptionDTO dto)
        {
            var entity = _mapper.Map<Subscription>(dto);
            return await _subscriptionRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _subscriptionRepository.DeleteAsync(
                await _subscriptionRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Subscription>> GetAllAsync(
            bool statement, int pageIndex, int pageSize)
        {
            var entities = await _subscriptionRepository.GetAllAsync(x => statement ? true : !x.IsDeleted);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Subscription>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Subscription> GetByIdAsync(int id)
        {
            return await _subscriptionRepository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RecoverAsync(int id)
        {
            await _subscriptionRepository.RecoverAsync(
                await _subscriptionRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _subscriptionRepository.RemoveAsync(
                await _subscriptionRepository.GetByIdAsync(x => x.Id == id));
        }

        // Support Methods

        public async Task<bool> CheckSubscriptionAsync(string email)
        {
            return await _subscriptionRepository.GetByIdAsync(x => !x.IsDeleted && x.Email == email) != null;
        }
    }
}

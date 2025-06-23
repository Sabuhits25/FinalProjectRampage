using AutoMapper;
using Rampage.BLL.DTO_s.ContactDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class ContactService : IContactService
    {
        protected readonly IContactRepository _contactRepository;
        protected readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateContactDTO dto)
        {
            var entity = _mapper.Map<Contact>(dto);
            await _contactRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _contactRepository.DeleteAsync(
                await _contactRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Contact>> GetAllAsync(
            bool statement, int pageIndex, int pageSize)
        {
            var entities = await _contactRepository.GetAllAsync(x => statement ? true : !x.IsDeleted);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Contact>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RecoverAsync(int id)
        {
            await _contactRepository.RecoverAsync(
                await _contactRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _contactRepository.RemoveAsync(
                await _contactRepository.GetByIdAsync(x => x.Id == id));
        }
    }
}

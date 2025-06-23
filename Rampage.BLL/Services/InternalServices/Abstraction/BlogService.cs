using AutoMapper;
using Rampage.BLL.DTO_s.BlogDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class BlogService : IBlogService
    {
        protected readonly IBlogRepository _blogRepository;
        protected readonly IMapper _mapper;

        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<Blog> CreateAsync(CreateBlogDTO dto)
        {
            var entity = _mapper.Map<Blog>(dto);
            return await _blogRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _blogRepository.DeleteAsync(
                await _blogRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Blog>> GetAllAsync(
            bool statement, int pageIndex, int pageSize)
        {
            var entities = await _blogRepository.GetAllAsync(x => statement ? true : !x.IsDeleted);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Blog>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _blogRepository.GetByIdAsync(x => x.Id == id,
                t => t.Translations);
        }

        public async Task RecoverAsync(int id)
        {
            await _blogRepository.RecoverAsync(
                await _blogRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _blogRepository.RemoveAsync(
                await _blogRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<Blog> UpdateAsync(UpdateBlogDTO dto)
        {
            var entity = _mapper.Map(dto,
                await _blogRepository.GetByIdAsync(x => x.Id == dto.Id));

            return await _blogRepository.UpdateAsync(entity);
        }

        // Support Methods

        public async Task<ICollection<BlogDTO>> GetAllBlogsByTranslationAsync(string lang)
        {
            var categories = await _blogRepository.GetAllAsync(x => !x.IsDeleted, c => c.Translations);

            var localizedCategories = categories.Select(c => new BlogDTO
            {
                ImageUrl = c.ImageUrl,
                Author = c.Author,
                Title = c.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Title ??
                       c.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Title,
                Description = c.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Description ??
                       c.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Description,
            }).ToList();

            return localizedCategories;
        }
    }
}

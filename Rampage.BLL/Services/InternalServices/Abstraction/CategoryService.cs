using AutoMapper;
using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.Core.Models;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class CategoryService : ICategoryService
    {
        protected readonly ICategoryRepository _categoryRepository;
        protected readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateCategoryDTO dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(
                await _categoryRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Category>> GetAllAsync(
             bool statement, int pageIndex, int pageSize)
        {
            var entities = await _categoryRepository.GetAllAsync(x => statement ? true : !x.IsDeleted,
                t => t.Translations,
                p => p.ParentCategory.Translations);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Category>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(x => x.Id == id,
                p => p.ParentCategory,
                t => t.Translations);
        }

        public async Task RecoverAsync(int id)
        {
            await _categoryRepository.RecoverAsync(
                await _categoryRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _categoryRepository.RemoveAsync(
                await _categoryRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task UpdateAsync(UpdateCategoryDTO dto)
        {
            await _categoryRepository.UpdateAsync(
                _mapper.Map(dto,
                await _categoryRepository.GetByIdAsync(x => x.Id == dto.Id)));
        }

        // Support Functions

        public async Task<ICollection<CategoryDTO>> GetAllNotDeletedCategoriesAsync()
        {
            return (await _categoryRepository.GetAllAsync(x => !x.IsDeleted,
                t => t.Translations))
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Translations.FirstOrDefault().Name,
                    ParentCategoryId = c.ParentCategoryId,
                }).ToList();
        }

        public async Task<ICollection<CategoryDTO>> GetAllCategoriesByTranslationAsync(string lang)
        {
            var categories = await _categoryRepository.GetAllAsync(x => !x.IsDeleted, c => c.Translations);

            var localizedCategories = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                ParentCategoryId = c.ParentCategoryId,
                ImageUrl = c.ImageUrl,
                Name = c.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       c.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name
            }).ToList();

            return localizedCategories;
        }

    }
}

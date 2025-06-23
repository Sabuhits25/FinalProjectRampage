using AutoMapper;
using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class ColorService : IColorService
    {
        protected readonly IColorRepository _colorRepository;
        protected readonly IMapper _mapper;

        public ColorService(IColorRepository colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateColorDTO dto)
        {
            var entity = _mapper.Map<Color>(dto);
            await _colorRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _colorRepository.DeleteAsync(
                await _colorRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Color>> GetAllAsync(
             bool statement, int pageIndex, int pageSize)
        {
            var entities = await _colorRepository.GetAllAsync(x => statement ? true : !x.IsDeleted,
                t => t.Translations);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Color>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Color> GetByIdAsync(int id)
        {
            return await _colorRepository.GetByIdAsync(x => x.Id == id,
                t => t.Translations);
        }

        public async Task RecoverAsync(int id)
        {
            await _colorRepository.RecoverAsync(
                await _colorRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _colorRepository.RemoveAsync(
                await _colorRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task UpdateAsync(UpdateColorDTO dto)
        {
            await _colorRepository.UpdateAsync(
                _mapper.Map(dto,
                await _colorRepository.GetByIdAsync(x => x.Id == dto.Id)));
        }

        // Support Functions

        public async Task<ICollection<ColorDTO>> GetAllNotDeletedColorsAsync()
        {
            return (await _colorRepository.GetAllAsync(x => !x.IsDeleted,
                t => t.Translations))
                .Select(c => new ColorDTO
                {
                    Id = c.Id,
                    Name = c?.Translations?.FirstOrDefault()?.Name,
                }).ToList();
        }

        public async Task<ICollection<ColorDTO>> GetAllColorsByTranslationAsync(string lang)
        {
            var colors = await _colorRepository.GetAllAsync(x => !x.IsDeleted, c => c.Translations);

            var localizedCategories = colors.Select(c => new ColorDTO
            {
                Id = c.Id,
                Name = c.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       c.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name
            }).ToList();

            return localizedCategories;
        }
    }
}

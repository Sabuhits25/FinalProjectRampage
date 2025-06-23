using AutoMapper;
using Rampage.BLL.DTO_s.CommentDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Enums;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class CommentService : ICommentService
    {
        protected readonly ICommentRepository _commentRepository;
        protected readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<Comment> CreateAsync(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            return await _commentRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _commentRepository.DeleteAsync(
                await _commentRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<PaginatedQueryable<Comment>> GetAllAsync(
            bool statement, int pageIndex, int pageSize)
        {
            var entities = await _commentRepository.GetAllAsync(x => statement ? true : !x.IsDeleted);

            var totalRecords = entities.Count;
            var transactions = entities
                .OrderByDescending(t => t.UpdatedOn)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<Comment>(transactions.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(x => x.Id == id);
        }

        public async Task RecoverAsync(int id)
        {
            await _commentRepository.RecoverAsync(
                await _commentRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task RemoveAsync(int id)
        {
            await _commentRepository.RemoveAsync(
                await _commentRepository.GetByIdAsync(x => x.Id == id));
        }

        public async Task<Comment> UpdateAsync(UpdateCommentDTO dto)
        {
            var entity = _mapper.Map(dto,
                await _commentRepository.GetByIdAsync(x => x.Id == dto.Id));

            return await _commentRepository.UpdateAsync(entity);
        }

        // Support Methods

        public async Task<ICollection<Comment>> GetAllCommentsByTranslationAsync(string lang)
        {
            var langEnum = lang == "az" ? ELanguage.AZ : ELanguage.EN;

            var comments = await _commentRepository.GetAllAsync(x => !x.IsDeleted
                && x.Language == langEnum);

            return comments;
        }
    }
}


using Rampage.BLL.DTO_s.CommentDTO_s;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface ICommentService
    {
        Task<ICollection<Comment>> GetAllCommentsByTranslationAsync(string lang);
        Task<PaginatedQueryable<Comment>> GetAllAsync(
            bool statement, int pageIndex, int pageSize);
        Task<Comment> GetByIdAsync(int id);
        Task<Comment> CreateAsync(CreateCommentDTO dto);
        Task<Comment> UpdateAsync(UpdateCommentDTO dto);
        Task DeleteAsync(int id);
        Task RecoverAsync(int id);
        Task RemoveAsync(int id);
    }
}

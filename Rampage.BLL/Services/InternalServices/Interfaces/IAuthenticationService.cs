using Rampage.BLL.DTO_s.AuthenticationDTO_s;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;

namespace Rampage.BLL.Services.InternalServices.Interfaces
{
    public interface IAuthenticationService
    {
        Task<PaginatedQueryable<User>> GetAllUsersBySearchAsync(
           bool statement, string query,
           int pageIndex, int pageSize);
        Task<User> UpdateUserPasswordOrDetailsAsync(UpdateUserDTO dto);
        Task CheckEmailConfirmationAsync(EmailConfirmationDTO dto);
        Task RegisterAsync(RegisterDTO dto);
        Task LoginAsync(LoginDTO dto);
        Task LogoutAsync();
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rampage.BLL.DTO_s.AuthenticationDTO_s;
using Rampage.BLL.Services.Interfaces;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;
using Rampage.Core.Enums;
using Rampage.Core.Exceptions.AuthenticationExceptions;
using Rampage.Core.Exceptions.Commons;

namespace Rampage.BLL.Services.InternalServices.Abstraction
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IFileManagerService _fileManagerService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFileManagerService fileManagerService,
            IMailService mailService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileManagerService = fileManagerService;
            _mailService = mailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task LoginAsync(LoginDTO dto)
        {
            var user = await CheckLoginAsync(dto);

            await _signInManager.SignInAsync(user, true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            var emailAddress = dto.Email;
            var newUser = _mapper.Map<User>(dto);
            newUser.UserName = $"{dto.FirstName}{dto.LastName}{new Random().Next(0, 99999)}";
            var number = GenerateConfirmationNumber();

            newUser.ConfirmationNumber = number;

            CheckIdentityResult(await _userManager.CreateAsync(newUser, dto.Password));
            CheckIdentityResult(await _userManager.AddToRoleAsync(newUser, EUserRole.Customer.ToString()));

            await _mailService.SendConfirmationCodeMessageAsync(number, emailAddress, newUser);
        }

        public async Task<User> UpdateUserPasswordOrDetailsAsync(UpdateUserDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (dto.PasswordOrDetail)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                CheckIdentityResult(await _userManager.ResetPasswordAsync(user, token, dto.NewPassword));
            }
            else
            {
                user.PhoneNumber = dto.PhoneNumber;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Address = dto.Address;
                user.City = dto.City;

                if (dto.Email != user.Email)
                {
                    var number = GenerateConfirmationNumber();

                    user.ConfirmationNumber = number;
                    user.Email = dto.Email;
                    user.EmailConfirmed = false;

                    await _mailService.SendConfirmationCodeMessageAsync(number, dto.Email, user);
                }

                CheckIdentityResult(await _userManager.UpdateAsync(user));
            }

            return user;
        }

        public async Task CheckEmailConfirmationAsync(EmailConfirmationDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (dto.Code == user.ConfirmationNumber)
            {
                user.EmailConfirmed = true;
                user.ConfirmationNumber = null;
                CheckIdentityResult(await _userManager.UpdateAsync(user));
            }
            else throw new Exception();
        }

        // Support Methods
        public async Task<PaginatedQueryable<User>> GetAllUsersBySearchAsync(
           bool statement, string query,
           int pageIndex, int pageSize)
        {
            var entities = await _userManager.Users
                .Where(x => (statement ? true : !x.IsDeleted)
                && x.UserName != "admin")
                .ToListAsync();

            if (!string.IsNullOrEmpty(query))
            {
                entities = entities.Where(t =>
                    t.UserName.ToLower().Contains(query.ToLower()) ||
                    t.Email.ToLower().Contains(query.ToLower()) ||
                    (t.FirstName != null && t.FirstName
                    .ToLower().Contains(query.ToLower())) ||
                     (t.LastName != null && t.LastName
                    .ToLower().Contains(query.ToLower()))
                ).ToList();
            }

            var totalRecords = entities.Count;
            var users = entities
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var data = new PaginatedQueryable<User>(users.AsQueryable(), pageIndex, totalPages);

            return data;
        }

        public void CheckIdentityResult(IdentityResult result)
        {
            if (result.Errors.Any(e => e.Code == "TokenExpired"))
                throw new Exception();

            if (!result.Succeeded)
                throw new Exception($"{result.Errors.FirstOrDefault()?.Description}");
        }

        public async Task<User> CheckLoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                throw new Exception("Username/Email address or password is not valid!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);

            if (result.IsLockedOut)
            {
                throw new Exception("Your account is lockout, please try again later!");
            }

            if (!result.Succeeded)
            {
                throw new Exception("Username/Email address or password is not valid!");
            }

            if (user.IsDeleted)
            {
                throw new Exception("Your account has been deleted by admin!");
            }

            return user;
        }

        public async Task CheckUserPasswordAsync(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (!user.EmailConfirmed)
                throw new EmailIsNotConfirmedException();

            if (!result.Succeeded)
                throw new EntityNotFoundException(user.UserName);
        }

        public int GenerateConfirmationNumber()
        {
            Random random = new Random();
            var digits = Enumerable.Range(0, 10).OrderBy(x => random.Next()).Take(6).ToArray();

            while (digits[0] == 0)
            {
                digits = Enumerable.Range(0, 10).OrderBy(x => random.Next()).Take(6).ToArray();
            }

            return int.Parse(string.Join("", digits));
        }
    }
}

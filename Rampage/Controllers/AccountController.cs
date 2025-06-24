using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.AuthenticationDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities.Identity;

namespace Rampage.Controllers
{
    public class AccountController : Controller
    {
        protected readonly IAuthenticationService _authenticationService;
        protected readonly UserManager<User> _userManager;

        public AccountController(IAuthenticationService authenticationService, UserManager<User> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Component()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dto = new UpdateUserDTO();

            dto.PhoneNumber = user.PhoneNumber;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            dto.Address = user.Address;
            dto.Email = user.Email;
            dto.UserId = user.Id;
            dto.City = user.City;

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Component(UpdateUserDTO dto)
        {
            try
            {
                if (dto.NewPassword is not null || dto.ConfirmNewPassword is not null) dto.PasswordOrDetail = true;
                else dto.PasswordOrDetail = false;

                var result = await _authenticationService.UpdateUserPasswordOrDetailsAsync(dto);

                if (!result.EmailConfirmed)
                {
                    await _authenticationService.LogoutAsync();

                    return RedirectToAction("EmailConfirmation", new { email = result.Email });
                }

                dto.PhoneNumber = result.PhoneNumber;
                dto.FirstName = result.FirstName;
                dto.LastName = result.LastName;
                dto.Address = result.Address;
                dto.Email = result.Email;
                dto.City = result.City;
                dto.UserId = result.Id;

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            {
                await _authenticationService.RegisterAsync(dto);

                return RedirectToAction("EmailConfirmation", new { email = dto.Email });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult EmailConfirmation(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationDTO dto)
        {
            await _authenticationService.CheckEmailConfirmationAsync(dto);

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                await _authenticationService.LoginAsync(dto);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.UserDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities.Identity;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        protected readonly IAuthenticationService _authenticationService;
        protected readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager, IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<IActionResult> Table(string query = "", int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                TempData["CurrentQuery"] = query;

                var statement = User.IsInRole("Admin");
                var transactionsQuery = await _authenticationService.GetAllUsersBySearchAsync(
                    statement, query, pageIndex, pageSize);

                ViewBag.PageSize = pageSize;
                TempData.Keep("CurrentQuery");

                return View(transactionsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId, int pageIndex = 1)
        {
            try
            {
                var oldUser = await _userManager.FindByIdAsync(userId);
                oldUser.IsDeleted = true;

                await _userManager.UpdateAsync(oldUser);

                TempData["SuccessMessage"] = "User message deleted successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the user message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string userId, int pageIndex = 1)
        {
            try
            {
                await _userManager.DeleteAsync(
                    await _userManager.FindByIdAsync(userId));

                TempData["SuccessMessage"] = "User message removed successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the user message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Recover(string userId, int pageIndex = 1)
        {
            try
            {
                var oldUser = await _userManager.FindByIdAsync(userId);
                oldUser.IsDeleted = false;

                await _userManager.UpdateAsync(oldUser);

                TempData["SuccessMessage"] = "User message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the user message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Component(UserComponentDTO dto, int pageIndex = 1)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);

                dto.IsEmailConfirmed = user.EmailConfirmed;
                dto.FirstName = user.FirstName;
                dto.LastName = user.LastName;
                dto.Username = user.UserName;
                dto.Phone = user.PhoneNumber;
                dto.Address = user.Address;
                dto.Email = user.Email;
                dto.City = user.City;

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Component(UserComponentDTO dto)
        {
            try
            {
                var oldUser = await _userManager.FindByIdAsync(dto.UserId);

                oldUser.EmailConfirmed = dto.IsEmailConfirmed;
                oldUser.FirstName = dto.FirstName;
                oldUser.LastName = dto.LastName;
                oldUser.UserName = dto.Username;
                oldUser.Address = dto.Address;
                oldUser.PhoneNumber = dto.Phone;
                oldUser.Email = dto.Email;
                oldUser.City = dto.City;

                await _userManager.UpdateAsync(oldUser);

                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            oldUser.EmailConfirmed = true;

            await _userManager.UpdateAsync(oldUser);

            return RedirectToAction(nameof(Component), new { userId = userId });
        }
    }
}

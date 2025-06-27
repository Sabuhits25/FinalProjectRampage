using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ContactDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ContactController : Controller
    {
        protected readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = User.IsInRole("Admin");
                var contactsQuery = await _contactService.GetAllAsync(
                    statement, pageIndex, pageSize);

                return View(contactsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the contact messages: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id, int pageIndex = 1)
        {
            try
            {
                await _contactService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Contact message deleted successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the contact message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id, int pageIndex = 1)
        {
            try
            {
                await _contactService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Contact message removed successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the contact message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id, int pageIndex = 1)
        {
            try
            {
                await _contactService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Contact message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the contact message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Component(int id, int pageIndex = 1)
        {
            try
            {
                var entity = await _contactService.GetByIdAsync(id);

                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Contact message not found.";
                    return RedirectToAction("Table");
                }

                return View(new ContactDTO
                {
                    FullName = entity.FullName,
                    Message = entity.Message,
                    Email = entity.Email,
                    PageIndex = pageIndex,
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the contact message details: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }
    }
}

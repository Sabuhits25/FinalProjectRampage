using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SubscriptionController : Controller
    {
        protected readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = true; // User.IsInRole("Admin");
                var subscriptionsQuery = await _subscriptionService.GetAllAsync(
                    statement, pageIndex, pageSize);

                return View(subscriptionsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id, int pageIndex = 1)
        {
            try
            {
                await _subscriptionService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Subscription message deleted successfully.";
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
                await _subscriptionService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Subscription message removed successfully.";
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
                await _subscriptionService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Subscription message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the contact message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }
    }
}

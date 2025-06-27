using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.TransactionDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Entities.Identity;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TransactionController : Controller
    {
        protected readonly ITransactionService _transactionService;
        protected readonly UserManager<User> _userManager;

        public TransactionController(UserManager<User> userManager, ITransactionService transactionService)
        {
            _userManager = userManager;
            _transactionService = transactionService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(string query = "", string status = "", int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                TempData["CurrentQuery"] = query;
                TempData["CurrentStatus"] = status;

                var statement = User.IsInRole("Admin");
                var transactionsQuery = await _transactionService.GetAllAsync(
                    statement, query, status, pageIndex, pageSize);

                ViewBag.PageSize = pageSize;
                TempData.Keep("CurrentQuery");
                TempData.Keep("CurrentStatus");

                return View(transactionsQuery);
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
                await _transactionService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Transaction message deleted successfully.";
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
                await _transactionService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Transaction message removed successfully.";
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
                await _transactionService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Transaction message recovered successfully.";
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
                var entity = await _transactionService.GetByIdAsync(id);

                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Transaction message not found.";
                    return RedirectToAction("Table");
                }

                return View(new TransactionDTO
                {
                    User = entity?.User,
                    OrderId = entity.OrderId,
                    Amount = entity.Amount,
                    SessionId = entity.SessionId,
                    CheckToken = entity.CheckToken,
                    Status = entity.Status.ToString(),
                    ResponseBody = entity.ResponseBody,
                    UpdatedOn = entity.UpdatedOn,
                    FullName = entity.FullName ?? ($"{entity?.User?.FirstName} {entity?.User?.FirstName}"
                    ?? entity?.User?.UserName),
                    Phone = entity?.Phone,
                    Email = entity?.Email,
                    City = entity?.City,
                    Address = entity?.Address,
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.CommentDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CommentController : Controller
    {
        protected readonly ICommentService _commentService;
        protected readonly IProductService _productService;

        public CommentController(ICommentService categoryService, IProductService productService)
        {
            _commentService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = true; // User.IsInRole("Admin");
                var CommentsQuery = await _commentService.GetAllAsync(
                    statement, pageIndex, pageSize);

                return View(CommentsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the comment messages: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id, int pageIndex = 1)
        {
            try
            {
                await _commentService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Comment message deleted successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the comment message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id, int pageIndex = 1)
        {
            try
            {
                await _commentService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Comment message removed successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the comment message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id, int pageIndex = 1)
        {
            try
            {
                await _commentService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Comment message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the comment message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productService.GetNotDeletedProductsAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CreateCommentDTO dto)
        {
            try
            {
                await _commentService.CreateAsync(dto);

                TempData["SuccessMessage"] = "Comment created successfully.";
                return RedirectToAction("Table");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while creating the comment: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Products = await _productService.GetNotDeletedProductsAsync();
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCommentDTO dto, int pageIndex = 1)
        {
            try
            {
                var oldComment = await _commentService.GetByIdAsync(dto.Id);
                ViewBag.Products = await _productService.GetNotDeletedProductsAsync();

                if (oldComment == null)
                {
                    TempData["ErrorMessage"] = "Comment not found.";
                    return RedirectToAction("Table");
                }

                var model = new UpdateCommentDTO
                {
                    Id = oldComment.Id,
                    Star = oldComment.Star,
                    Message = oldComment.Message,
                    Author = oldComment.Author,
                    Name = oldComment.Name,
                    ImageUrl = oldComment.ImageUrl,
                    PageIndex = pageIndex,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the comment: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Products = await _productService.GetNotDeletedProductsAsync();
                return RedirectToAction("Table");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCommentDTO dto)
        {
            try
            {
                await _commentService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Comment updated successfully.";
                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the comment: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Products = await _productService.GetNotDeletedProductsAsync();
                return View(dto);
            }
        }
    }
}

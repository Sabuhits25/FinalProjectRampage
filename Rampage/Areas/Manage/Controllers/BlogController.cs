using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.BlogDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{

    [Area("Manage")]
    public class BlogController : Controller
    {

        protected readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = User.IsInRole("Admin");
                var blogsQuery = await _blogService.GetAllAsync(
                    statement, pageIndex, pageSize);

                return View(blogsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the blog messages: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id, int pageIndex = 1)
        {
            try
            {
                await _blogService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Blog message deleted successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the blog message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id, int pageIndex = 1)
        {
            try
            {
                await _blogService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Blog message removed successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the blog message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id, int pageIndex = 1)
        {
            try
            {
                await _blogService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Blog message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the blog message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CreateBlogDTO dto)
        {
            try
            {
                var newBlog = await _blogService.CreateAsync(dto);

                TempData["SuccessMessage"] = "Blog created successfully.";
                return RedirectToAction("Table");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while creating blog: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateBlogDTO dto, int pageIndex = 1)
        {
            try
            {
                var oldBlog = await _blogService.GetByIdAsync(dto.Id);

                if (oldBlog == null)
                {
                    TempData["ErrorMessage"] = "Blog not found.";
                    return RedirectToAction("Table");
                }

                var translations = oldBlog.Translations.Select(t => new UpdateBlogTranslationDTO
                {
                    Id = t?.Id ?? 0,
                    BlogId = oldBlog.Id,
                    Description = t?.Description ?? string.Empty,
                    Title = t.Title ?? string.Empty,
                    Language = t.Language,
                }).ToList();

                var model = new UpdateBlogDTO
                {
                    Id = oldBlog.Id,
                    Author = oldBlog.Author,
                    ImageUrl = oldBlog.ImageUrl,
                    Translations = translations,
                    PageIndex = pageIndex,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the blog update page: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateBlogDTO dto)
        {
            try
            {
                await _blogService.UpdateAsync(dto);

                TempData["SuccessMessage"] = "Blog updated successfully.";
                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the blog: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }
    }
}

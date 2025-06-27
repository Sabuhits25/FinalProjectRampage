using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        protected readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = User.IsInRole("Admin");
                var contactsQuery = await _categoryService.GetAllAsync(
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
                await _categoryService.DeleteAsync(id);
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
                await _categoryService.RemoveAsync(id);
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
                await _categoryService.RecoverAsync(id);
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
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllNotDeletedCategoriesAsync();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CreateCategoryDTO dto)
        {
            try
            {
                await _categoryService.CreateAsync(dto);

                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction("Table");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while creating the Category: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCategoryDTO dto, int pageIndex = 1)
        {
            try
            {
                var oldCategory = await _categoryService.GetByIdAsync(dto.Id);
                var categories = await _categoryService.GetAllNotDeletedCategoriesAsync();

                if (oldCategory == null)
                {
                    TempData["ErrorMessage"] = "Category not found.";
                    return RedirectToAction("Table");
                }

                var translations = oldCategory.Translations.Select(t => new UpdateCategoryTranslationDTO
                {
                    Id = t?.Id ?? 0,
                    Name = t?.Name ?? string.Empty,
                    CategoryId = t?.CategoryId ?? 0,
                    Language = t.Language,
                }).ToList();

                var model = new UpdateCategoryDTO
                {
                    Id = oldCategory.Id,
                    ParentCategoryId = oldCategory?.ParentCategory?.Id,
                    Categories = categories,
                    Translations = translations,
                    ImageUrl = oldCategory.ImageUrl,
                    PageIndex = pageIndex,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the Category: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCategoryDTO dto)
        {
            try
            {
                await _categoryService.UpdateAsync(dto);

                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the Category: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }
    }
}

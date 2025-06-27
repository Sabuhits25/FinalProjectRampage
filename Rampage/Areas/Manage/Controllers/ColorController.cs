using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ColorController : Controller
    {
        protected readonly IColorService _colorService;

        public ColorController(IColorService categoryService)
        {
            _colorService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = true;// User.IsInRole("Admin");
                var colorsQuery = await _colorService.GetAllAsync(
                    statement, pageIndex, pageSize);

                return View(colorsQuery);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the color messages: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Delete(int id, int pageIndex = 1)
        {
            try
            {
                await _colorService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Color message deleted successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while deleting the color message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id, int pageIndex = 1)
        {
            try
            {
                await _colorService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Color message removed successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while removing the color message: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table", new { pageIndex });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id, int pageIndex = 1)
        {
            try
            {
                await _colorService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Color message recovered successfully.";
                return RedirectToAction("Table", new { pageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while recovering the color message: \n{ex.Message}\n{ex.InnerException?.Message}";
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
        public async Task<IActionResult> Create(CreateColorDTO dto)
        {
            try
            {
                await _colorService.CreateAsync(dto);

                TempData["SuccessMessage"] = "Color created successfully.";
                return RedirectToAction("Table");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while creating the Color: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateColorDTO dto, int pageIndex = 1)
        {
            try
            {
                var oldColor = await _colorService.GetByIdAsync(dto.Id);

                if (oldColor == null)
                {
                    TempData["ErrorMessage"] = "Color not found.";
                    return RedirectToAction("Table");
                }

                var translations = oldColor.Translations.Select(t => new UpdateColorTranslationDTO
                {
                    Id = t?.Id ?? 0,
                    Name = t?.Name ?? string.Empty,
                    ColorId = t?.ColorId ?? 0,
                    Language = t.Language,
                }).ToList();

                var model = new UpdateColorDTO
                {
                    Id = oldColor.Id,
                    Translations = translations,
                    PageIndex = pageIndex,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the Color: \n{ex.Message}\n{ex.InnerException?.Message}";
                return RedirectToAction("Table");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateColorDTO dto)
        {
            try
            {
                await _colorService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Color updated successfully.";
                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the Color: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }
    }
}

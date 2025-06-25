using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{

    [Area("Manage")]
    public class ProductController : Controller
    {
        protected readonly IProductService _productService;
        protected readonly ICategoryService _categoryService;
        protected readonly IColorService _colorService;

        public ProductController(IProductService blogService,
            ICategoryService categoryService, IColorService colorService)
        {
            _productService = blogService;
            _categoryService = categoryService;
            _colorService = colorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table(int pageIndex = 1, int pageSize = 8)
        {
            try
            {
                var statement = true; // User.IsInRole("Admin");
                var blogsQuery = await _productService.GetAllAsync(
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
                await _productService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Product message deleted successfully.";
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
                await _productService.RemoveAsync(id);
                TempData["SuccessMessage"] = "Product message removed successfully.";
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
                await _productService.RecoverAsync(id);
                TempData["SuccessMessage"] = "Product message recovered successfully.";
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllNotDeletedCategoriesAsync();
            ViewBag.Colors = await _colorService.GetAllNotDeletedColorsAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CreateProductDTO dto)
        {
            try
            {
                var newProduct = await _productService.CreateAsync(dto);

                TempData["SuccessMessage"] = "Product created successfully.";
                return RedirectToAction("Table");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while creating blog: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Categories = await _categoryService.GetAllNotDeletedCategoriesAsync();
                ViewBag.Colors = await _colorService.GetAllNotDeletedColorsAsync();
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductDTO dto, int pageIndex = 1)
        {
            try
            {
                var oldProduct = await _productService.GetByIdAsync(dto.Id);
                ViewBag.Categories = await _categoryService.GetAllNotDeletedCategoriesAsync();
                ViewBag.Colors = await _colorService.GetAllNotDeletedColorsAsync();

                if (oldProduct == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction("Table");
                }

                var translations = oldProduct.Translations.Select(t => new UpdateProductTranslationDTO
                {
                    Id = t?.Id ?? 0,
                    ProductId = oldProduct.Id,
                    Description = t?.Description ?? string.Empty,
                    Name = t?.Name ?? string.Empty,
                    Language = t.Language,
                }).ToList();

                var settings = oldProduct.Settings.Select(s => new UpdateProductSettingDTO
                {
                    Id = s.Id,
                    Key = s.Key,
                    Value = s.Value,
                    Language = s.Language,
                }).ToList();

                var model = new UpdateProductDTO
                {
                    Id = oldProduct.Id,
                    Code = oldProduct.Code,
                    BarCode = oldProduct.BarCode,
                    Star = oldProduct.Star,
                    Price = oldProduct.Price,
                    CategoryId = oldProduct.CategoryId,
                    ColorIds = oldProduct?.Colors?.Select(c => c.ColorId)?.ToList(),
                    CurrentImages = oldProduct?.Images?.Select(i => new CurrentProductImageDTO
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                    }).ToList(),
                    Translations = translations,
                    Settings = settings,
                    PageIndex = pageIndex,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the blog update page: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Categories = await _categoryService.GetAllNotDeletedCategoriesAsync();
                ViewBag.Colors = await _colorService.GetAllNotDeletedColorsAsync();

                return RedirectToAction("Table");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductDTO dto, string removedImageIds)
        {
            try
            {
                dto.RemovedProductImageIds = removedImageIds;
                await _productService.UpdateAsync(dto);

                TempData["SuccessMessage"] = "Product updated successfully.";
                return RedirectToAction("Table", new { pageIndex = dto.PageIndex });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the blog: \n{ex.Message}\n{ex.InnerException?.Message}";
                ViewBag.Categories = await _categoryService.GetAllNotDeletedCategoriesAsync();
                ViewBag.Colors = await _colorService.GetAllNotDeletedColorsAsync();

                return View(dto);
            }
        }
    }
}

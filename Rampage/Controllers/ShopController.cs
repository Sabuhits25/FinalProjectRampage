using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Enums;

namespace Rampage.Controllers
{
    public class ShopController : Controller
    {
        protected readonly IProductService _productService;
        protected readonly ICategoryService _categoryService;
        protected readonly IColorService _colorService;

        public ShopController(IProductService productService, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int categoryId)
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            var products = categoryId != 0 ? await _productService.GetAllProductsByCategoryAsync(categoryId) :
                await _productService.GetNotDeletedProductsAsync();
            var categories = await _categoryService.GetAllCategoriesByTranslationAsync(lang);
            var colors = await _colorService.GetAllColorsByTranslationAsync(lang);
            var category = await _categoryService.GetByIdAsync(categoryId);

            var filterProductDTO = new FilterProductDTO
            {
                Colors = colors,
                Products = products,
                Categories = categories,
                CategoryName = category?.Translations?.FirstOrDefault(t => t.Language.ToString()?.ToLower() == lang)?.Name,
                CategoryImageUrl = category?.ImageUrl,
            };

            return View(filterProductDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Index(FilterProductDTO dto)
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            // Get all products initially
            var categories = await _categoryService.GetAllCategoriesByTranslationAsync(lang);
            var colors = await _colorService.GetAllColorsByTranslationAsync(lang);
            var products = await _productService.GetNotDeletedProductsAsync();
            var categoryName = string.Empty;
            var categoryImageUrl = string.Empty;

            if (dto.CategoryId != 0)
            {
                var category = await _categoryService.GetByIdAsync(dto.CategoryId);
                categoryName =
                   category.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name;
                categoryImageUrl = category.ImageUrl;
            }

            if (dto.CategoryId != 0)
            {
                products = products.Where(p => p.CategoryId == dto.CategoryId).ToList();
            }

            if (dto.MinPrice > 0)
            {
                products = products.Where(p => p.Price >= dto.MinPrice).ToList();
            }

            if (dto.MaxPrice > 0 && dto.MaxPrice >= dto.MinPrice)
            {
                products = products.Where(p => p.Price <= dto.MaxPrice).ToList();
            }

            if (dto.ColorIds != null && dto.ColorIds.Any())
            {
                products = products.Where(p => p.Colors.Any(c => dto.ColorIds.Contains(c.ColorId))).ToList();
            }

            var filterProductDTO = new FilterProductDTO
            {
                Colors = colors,
                Products = products,
                ColorIds = dto.ColorIds,
                Categories = categories,
                MinPrice = dto.MinPrice,
                MaxPrice = dto.MaxPrice,
                CategoryId = dto.CategoryId,
                CategoryImageUrl = string.IsNullOrEmpty(categoryImageUrl)
                ? null : categoryImageUrl,
                CategoryName = string.IsNullOrEmpty(categoryName)
                ? null : categoryName,
            };

            return View(filterProductDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Component(int id)
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";
            var products = (await _productService.GetNotDeletedProductsAsync()).Take(4);

            var product = await _productService.GetByIdAsync(id);
            var category = await _categoryService.GetByIdAsync(product.CategoryId);
            var categoryName = category.Translations.
                FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name;
            var categoryImageUrl = category.ImageUrl;

            var newProductDTO = new ProductDTO
            {
                Id = id,
                Name = product.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       product.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                Description = product.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Description ??
                       product.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Description,
                Star = product.Star,
                Code = product.Code,
                BarCode = product.BarCode,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = categoryName,
                CategoryImageUrl = categoryImageUrl,
                ImageUrl = product.Images.FirstOrDefault().ImageUrl,
                ImageUrls = product.Images.Select(t => t.ImageUrl).ToList(),
                Settings = product.Settings?
                    .Where(s => s.Language.ToString().ToLower() == lang)
                    .ToDictionary(s => s.Key, s => s.Value) ??
                           product.Settings?
                    .Where(s => s.Language == ELanguage.EN)
                    .ToDictionary(s => s.Key, s => s.Value) ?? new Dictionary<string, string>()
            };

            var localizedProducts = products.Select(p => new ProductDTO
            {
                ImageUrl = p.Images.FirstOrDefault().ImageUrl,
                Price = p.Price,
                Star = p.Star,
                Name = p.Translations.FirstOrDefault(t => t.Language.ToString().ToLower() == lang)?.Name ??
                       p.Translations.FirstOrDefault(t => t.Language == ELanguage.EN)?.Name,
                CategoryId = p.CategoryId,
                Id = p.Id,
            }).ToList();

            var newProductComponentDTO = new ProductComponentDTO
            {
                Product = newProductDTO,
                Products = localizedProducts,
            };

            return View(newProductComponentDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string code)
        {
            var productId = await _productService.GetProductIdBySearchAsync(code);

            if (productId == 0) return RedirectToAction("Index");

            return RedirectToAction("Component", new { id = productId });
        }
    }
}

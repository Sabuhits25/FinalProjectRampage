using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;
using Rampage.Core.Enums;

namespace Rampage.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IProductService _productService;
        protected readonly ICategoryService _categoryService;
        protected readonly ICommentService _commentService;

        public HomeController(IProductService productService, ICategoryService categoryService, ICommentService commentService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";
            var products = (await _productService.GetNotDeletedProductsAsync()).Take(4);
            ViewBag.Categories = (await _categoryService.GetAllCategoriesByTranslationAsync(lang)).Take(3);

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

            ViewBag.Comments = await _commentService.GetAllCommentsByTranslationAsync(lang);

            return View(localizedProducts);
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            ViewBag.Comments = await _commentService.GetAllCommentsByTranslationAsync(lang);

            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404)
                {
                    return View("NotFound");
                }
                else
                {
                    return View("GenError");
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}

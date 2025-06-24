using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Controllers
{
    public class BlogController : Controller
    {
        protected readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            ViewBag.Blogs = await _blogService.GetAllBlogsByTranslationAsync(lang);

            return View();
        }
    }
}

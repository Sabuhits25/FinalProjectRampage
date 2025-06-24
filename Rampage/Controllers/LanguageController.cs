using Microsoft.AspNetCore.Mvc;

namespace Rampage.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string lang, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/" + lang);
            }

            var segments = returnUrl.Trim('/').Split('/');
            if (segments.Length > 0 && (segments[0] == "en" || segments[0] == "az"))
            {
                segments[0] = lang;
            }
            else
            {
                return Redirect("/" + lang);
            }

            string newPath = "/" + string.Join("/", segments);
            return Redirect(newPath);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.ContactDTO_s;
using Rampage.BLL.DTO_s.SubscriptionDTO_s;
using Rampage.BLL.Services.InternalServices.Interfaces;

namespace Rampage.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ISubscriptionService _subscriptionService;

        public ContactUsController(IContactService contactService, ISubscriptionService subscriptionService)
        {
            _contactService = contactService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateContactDTO dto)
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            if (ModelState.IsValid)
            {
                try
                {
                    await _contactService.CreateAsync(dto);
                    TempData["SuccessMessage"] = lang == "az" ? "Mesajınız uğurla göndərildi!" : "Your message has been sent successfully!";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = lang == "az" ? "Xəta baş verdi, zəhmət olmasa yenidən yoxlayın." :
                        "An error occurred, please check again.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = lang == "az" ? "Zəhmət olmasa məlumatları düzgün doldurun." :
                    "Please fill in the information correctly.";
            }
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            var lang = HttpContext.Items["lang"]?.ToString() ?? "en";

            if (email is null) RedirectToAction("Index", "Home");

            var subscribedEmail = await _subscriptionService.CheckSubscriptionAsync(email);

            if (subscribedEmail)
            {
                TempData["SubscribeMessage"] = lang == "az" ? "Qeyd etdiyiniz mail adresi artıq abonə olub!" :
                   "The email address you entered is already subscribed!";

                return RedirectToAction("Index", "Home");
            }

            var newCreateSubscribeDTO = new CreateSubscriptionDTO { Email = email };

            await _subscriptionService.CreateAsync(newCreateSubscribeDTO);

            TempData["SubscribeMessage"] = lang == "az" ? "Veb saytımıza uğurla abonə oldunuz, yeni məhsullardan xəbərdarlıq gözləyin :D" :
                    "You have successfully subscribed to the website, stay tuned for notifications of new products :D";

            return RedirectToAction("Index", "Home");
        }
    }
}

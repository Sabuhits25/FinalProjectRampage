using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rampage.BLL.DTO_s.SettingDTO_s;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin, Moderator")]
    public class SettingController : Controller
    {
        protected readonly ISettingRepository _settingRepository;
        protected readonly IMapper _mapper;

        public SettingController(ISettingRepository settingRepository,
            IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            try
            {
                var settings = (await _settingRepository.GetAllAsync(x => true))
                    .FirstOrDefault();

                if (settings == null)
                {
                    TempData["ErrorMessage"] = "Settings not found.";
                    return RedirectToAction("Index", "Dashboard");
                }

                var model = new SettingDTO
                {
                    Address = settings.Address,
                    Phone = settings.Phone,
                    Email = settings.Email,
                    Facebook = settings.Facebook,

                    LinkedIn = settings.LinkedIn,
                    Instagram = settings.Instagram,
                    Youtube = settings.Youtube,
                    DailyUsers = settings.DailyUsers,
                    DailyProducts = settings.DailyProducts,
                    CompanyCount = settings.CompanyCount,
                    CustomerCount = settings.CustomerCount,
                    ImageUrl = settings.ImageUrl,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the settings update page: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SettingDTO dto)
        {
            try
            {
                var oldSettings = _mapper.Map(dto,
                    (await _settingRepository.GetAllAsync(x => true))
                    .FirstOrDefault());

                await _settingRepository.UpdateAsync(oldSettings);

                TempData["SuccessMessage"] = "Settings updated successfully.";
                return RedirectToAction("Update");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the settings: \n{ex.Message}\n{ex.InnerException?.Message}";
                return View(dto);
            }
        }
    }
}

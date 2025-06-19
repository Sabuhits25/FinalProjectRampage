using Microsoft.AspNetCore.Http;
using Rampage.BLL.DTO_s.Commons;

namespace Rampage.BLL.DTO_s.SettingDTO_s
{
    public class SettingDTO : IAuditedEntityDTO
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public int DailyUsers { get; set; }
        public byte DailyProducts { get; set; }
        public byte CompanyCount { get; set; }
        public int CustomerCount { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}

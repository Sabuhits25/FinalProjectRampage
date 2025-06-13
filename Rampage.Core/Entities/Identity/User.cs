using Microsoft.AspNetCore.Identity;

namespace Rampage.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? ConfirmationNumber { get; set; }
        public bool IsDeleted { get; set; }

    }
}

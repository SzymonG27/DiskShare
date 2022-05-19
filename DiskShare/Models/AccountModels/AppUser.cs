using Microsoft.AspNetCore.Identity;

namespace DiskShare.Models.AccountModels
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string SurName { get; set; } = null!;
    }
}

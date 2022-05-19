using System.ComponentModel.DataAnnotations;

namespace DiskShare.Models.AccountModels
{
    public class Login
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = null!;

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}

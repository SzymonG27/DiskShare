using System.ComponentModel.DataAnnotations;

namespace DiskShare.Models.AccountModels
{
    public class Register
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; } = null!;

        [Required]
        [Display(Name = "Imię użytkownika")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Nazwisko użytkownika")]
        public string SurName { get; set; } = null!;

        [Required]
        [StringLength(999, ErrorMessage = "Hasło musi mieć minimum {2} znaków długości.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie nie są takie same.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

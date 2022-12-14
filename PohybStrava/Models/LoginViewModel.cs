using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [Display(Name = "Zapamatovat si heslo")]
        public bool RememberMe { get; set; }
    }

}

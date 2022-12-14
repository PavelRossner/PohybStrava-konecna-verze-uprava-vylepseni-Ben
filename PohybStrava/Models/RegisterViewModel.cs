using Microsoft.AspNetCore.Identity;
using PohybStrava.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class RegisterViewModel : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]

        [Display(Name = "Id")]
        public string UserId { get; set; } = "";

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public override string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "Délka {0} musí být alespoň {2} znaky a nejvíce {1} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Potvrď heslo")]
        [Compare("Password", ErrorMessage = "Hesla musí být stejná.")]
        public string ConfirmPassword { get; set; } = "";


        [Display(Name = "Jméno")]
        public string FirstName { get; set; } = "";

        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = "";

        [Display(Name = "Datum narození")]
        public DateTime DateUser { get; set; }

        [Display(Name = "Pohlaví (muž/žena)")]
        public string Gender { get; set; }



        public ICollection<Diet> Diets { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<EnergyBalanceResponse> EnergyBalance { get; set; }



    }
}

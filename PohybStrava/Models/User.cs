using Microsoft.AspNetCore.Identity;
using PohybStrava.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]

        [Display(Name = "Email")]
        public override string Email { get; set; } = "";

        [Display(Name = "Jméno")]
        public string FirstName { get; set; } = "";

        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = "";

        [Display(Name = "Pohlaví")]
        public virtual string Gender { get; set; } = "";

        [Display(Name = "Datum narození")]
        public DateTime DateOfBirth { get; set; }

        //Navigation properties
        public virtual ICollection<Diet> Diet { get; set; } = new HashSet<Diet>();
        public virtual ICollection<Activity> Activity { get; set; } = new HashSet<Activity>();
        public virtual ICollection<Stats> Stats { get; set; } = new HashSet<Stats>();
        public virtual ICollection<FoodDatabase> FoodDatabase { get; set; } = new HashSet<FoodDatabase>();

    }
}


//https://www.learnentityframeworkcore.com/relationships
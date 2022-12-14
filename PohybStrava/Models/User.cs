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

        //public int day;
        //public int month;
        //public int year;


        //public int Day
        //{
        //    get { return int.Parse(DateOfBirth.ToString("dd")); }
        //    set { day = value; }
        //}

        //[Display(Name = "Měsíc")]
        //public int Month
        //{
        //    get { return int.Parse(DateOfBirth.ToString("MM")); }
        //    set { month = value; }
        //}

        //public int Year
        //{
        //    get { return int.Parse(DateOfBirth.ToString("yyyy")); }
        //    set { year = value; }
        //}


        //Navigation properties
        public virtual ICollection<Diet> Diet { get; set; } = new HashSet<Diet>();
        public virtual ICollection<Activity> Activities { get; set; } = new HashSet<Activity>();
        public virtual ICollection<StatsResponse> StatsResponse { get; set; } = new HashSet<StatsResponse>();
        public virtual ICollection<FoodDatabase> FoodDatabase { get; set; } = new HashSet<FoodDatabase>();

    }
}


//https://www.learnentityframeworkcore.com/relationships
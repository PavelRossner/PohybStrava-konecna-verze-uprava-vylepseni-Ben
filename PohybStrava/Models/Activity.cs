using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ActivityId { get; set; }
        
        //Navigation properties
        public string UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Display(Name = "Datum")]
        public DateTime DateActivity { get; set; }

        [Display(Name = "Trasa")]
        public string Trail { get; set; } = "";

        [Display(Name = "Vzdálenost (km)")]
        public double Distance { get; set; }

        [Display(Name = "Převýšení (m)")]
        public double Elevation { get; set; }

        [Display(Name = "Čas (hod., min.)")]
        public string Time { get; set; } = "";

        [Display(Name = "Tempo (min/km)")]
        public double Pace { get; set; }

        [Display(Name = "Vydaná energie (kcal)")]
        public double EnergyActivity { get; set; }

    }

}

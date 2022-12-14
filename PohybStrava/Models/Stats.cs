using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class Stats
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int StatsId { get; set; }

        //Navigation properties
        public string UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Datum")]
        public DateTime UserDate { get; set; }

        [Display(Name = "Věk")]
        public double Age { get; set; }

        [Display(Name = "Váha")]
        public double Weight { get; set; }

        [Display(Name = "Výška (cm)")]
        public double Height { get; set; }


    }
}

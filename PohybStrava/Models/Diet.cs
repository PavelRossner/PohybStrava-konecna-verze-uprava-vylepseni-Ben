using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class Diet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int DietId { get; set; }
        public string UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Display(Name = "Datum")]
        public DateTime DateDiet { get; set; }

        [Display(Name = "Potravina")]
        public string Food { get; set; }

        [Display(Name = "Energie (kcal)")]
        public double EnergyDiet { get; set; }

        [Display(Name = "Množství")]
        public double Amount { get; set; }

    }
}

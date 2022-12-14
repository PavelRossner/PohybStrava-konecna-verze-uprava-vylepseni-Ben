using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class EnergyBalanceResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int EnergyBalanceId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Display(Name = "Datum - strava")]
        public DateTime DietDate { get; set; }

        [Display(Name = "Datum - aktivita")]
        public DateTime ActivityDate { get; set; }


        [Display(Name = "Příjem energie")]
        public int EnergyDietTotal { get; set; }

        [Display(Name = "Výdej energie")]
        public int EnergyActivitesTotal { get; set; }

        [Display(Name = "Bazální metabolismus")]
        public double BMR { get; set; }

        [Display(Name = "Energetická bilance")]
        public double EnergyBalanceTotal
        {
            get { return EnergyDietTotal - (EnergyActivitesTotal + BMR); }
            set { energyBalanceTotal = value; }
        }

        public double energyBalanceTotal;
        public int day;
        public int month;
        public int year;

        public int Day
        {
            get { return int.Parse(DietDate.ToString("dd")); }
            set { month = value; }
        }

        public int Month
        {
            get { return int.Parse(DietDate.ToString("MM")); }
            set { month = value; }
        }

        public int Year
        {
            get { return int.Parse(DietDate.ToString("yyyy")); }
            set { year = value; }
        }

    }
}

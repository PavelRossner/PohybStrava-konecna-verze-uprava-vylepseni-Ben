using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class ActivityResponse : Activity
    {
        public int day;
        public int month;
        public int year;

        [Display(Name = "Den")]
        public int Day
        {
            get { return int.Parse(DateActivity.ToString("dd")); }
            set { day = value; }
        }

        [Display(Name = "Měsíc")]
        public int Month
        {
            get { return int.Parse(DateActivity.ToString("MM")); }
            set { month = value; }
        }

        public int Year
        {
            get { return int.Parse(DateActivity.ToString("yyyy")); }
            set { year = value; }
        }

        public int DistanceSum { get; set; }
        public int ElevationSum { get; set; }

        [Display(Name = "Vydaná energie (kcal)")]
        public int EnergyActivityTotal { get; set; }


    }
}

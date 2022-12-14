using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class DietResponse
    {
        public int DietId { get; set; }

        [Display(Name = "Datum")]
        public DateTime DateDiet { get; set; }
        
        [Display(Name = "Potravina")]
        public string Food { get; set; }

        [Display(Name = "Energie (kcal)")]
        public double EnergyDiet { get; set; }

        [Display(Name = "Množství")]
        public double Amount { get; set; }

        [Display(Name = "Energie celkem (kcal)")]
        public double EnergyDietFoodTotal { get; set; }

        public static DietResponse GetDietResponse(Diet diet)
        {
            return new DietResponse 
            {
                EnergyDiet= diet.EnergyDiet,
                Food= diet.Food,
                DateDiet = diet.DateDiet,
                Amount = diet.Amount,
                
            };
        }
    }
}

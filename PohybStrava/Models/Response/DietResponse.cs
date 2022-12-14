using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class DietResponse : Diet
    {

        [Display(Name = "Energie celkem (kcal)")]
        public double EnergyDietFoodTotal => (EnergyDiet * Amount);

        public int EnergyDietSum { get; set; }

        public int Day => int.Parse(DateDiet.ToString("dd"));

        public int Month => int.Parse(DateDiet.ToString("MM"));

        public int Year => int.Parse(DateDiet.ToString("yyyy"));





        public static DietResponse GetMonthlyOverviewResponse(Diet diet)
        {
            return new DietResponse
            {
                EnergyDiet = diet.EnergyDiet,
                DateDiet = diet.DateDiet,
                Amount = diet.Amount
            };
        }



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

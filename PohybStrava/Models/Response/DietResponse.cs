using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class DietResponse : Diet
    {
        //public int DietId { get; set; }

        public double energyDietFoodTotal;

        [Display(Name = "Energie celkem (kcal)")]
        public double EnergyDietFoodTotal => (EnergyDiet * Amount);

        //public double EnergyDietFoodTotal { get { return (EnergyDiet * Amount); } set { energyDietFoodTotal = value; } }

        public int EnergyDietSum { get; set; }

        public int Day => int.Parse(DateDiet.ToString("dd"));

        public int Month => int.Parse(DateDiet.ToString("MM"));

        public int Year => int.Parse(DateDiet.ToString("yyyy"));


        //public static DietResponse GetMonthlyOverviewResponse(Diet diet)
        //{
        //    return new DietResponse
        //    {
        //        DietId = diet.DietId,
        //        EnergyDiet = diet.EnergyDiet,
        //        DateDiet = diet.DateDiet,
        //        Amount = diet.Amount
        //    };
        //}

        public static DietResponse GetDietResponse(Diet diet)
        {
            return new DietResponse
            {
                DietId = diet.DietId,
                DateDiet = diet.DateDiet,
                Food = diet.Food,
                EnergyDiet = diet.EnergyDiet,               
                Amount = diet.Amount,
            };
        }
    }
}

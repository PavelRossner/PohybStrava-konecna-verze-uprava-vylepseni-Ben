﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PohybStrava.Models.Response
{
    public class DietMonthlyOverviewResponse
    {

        [Display(Name = "Datum")]
        public DateTime DateDiet { get; set; }

        [Display(Name = "Energie (kcal)")]
        public double EnergyDiet { get; set; }

        [Display(Name = "Množství")]
        public double Amount { get; set; }

        [Display(Name = "Energie celkem (kcal)")]
        public double EnergyDietFoodTotal => (EnergyDiet * Amount);

        public int EnergyDietSum { get; set; }

        public int Day => int.Parse(DateDiet.ToString("dd"));

        public int Month => int.Parse(DateDiet.ToString("MM"));

        public int Year => int.Parse(DateDiet.ToString("yyyy"));

        public static DietMonthlyOverviewResponse GetMonthlyOverviewResponse(Diet diet)
        {
            return new DietMonthlyOverviewResponse 
            {
                EnergyDiet = diet.EnergyDiet,
                DateDiet = diet.DateDiet,
                Amount = diet.Amount
            };
        }
    }
}
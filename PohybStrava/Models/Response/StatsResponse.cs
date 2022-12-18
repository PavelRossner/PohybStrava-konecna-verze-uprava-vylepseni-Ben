using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PohybStrava.Models.Response
{
    //[Keyless]
    public class StatsResponse:Stats
    {
        [Display(Name = "Pohlaví (muž/žena)")]
        public string Gender { get; set; } = "";


        private double bmi;
        private double bmr;


        [Display(Name = "BMI")]
        public double BMI
        {
            get { return Math.Round(Weight / (Height / 100 * (Height / 100)), 2); }
            set { bmi = value; }
        }

        [Display(Name = "Bazální metabolismus (kcal)")]
        public double BMR
        {
            get
            {
                {
                    if (Gender == "žena")
                    { return Math.Round(655.0955 + 9.5634 * Weight + 1.8496 * Height - 4.6756 * Age, 0); }

                    else
                    { return Math.Round(66.473 + 13.7516 * Weight + 5.0033 * Height - 6.755 * Age, 0); }
                };
            }
            set { bmr = value; }
        }

        public int day;
        public int month;
        public int year;

        public int Day
        {
            get { return int.Parse(UserDate.ToString("dd")); }
            set { month = value; }
        }

        public int Month
        {
            get { return int.Parse(UserDate.ToString("MM")); }
            set { month = value; }
        }

        public int Year
        {
            get { return int.Parse(UserDate.ToString("yyyy")); }
            set { year = value; }
        }

        public double WeightAverage { get; set; }
        public double BMIAverage { get; set; }

        public static StatsResponse GetStatsResponse(Stats stats)
        {
            return new StatsResponse
            {
                UserDate = stats.UserDate,
                Age = stats.Age,
                Weight = stats.Weight,
                Height = stats.Height,
                //BMI = stats.BMI,
                //BMR = stats.BMR,

            };
        }

    }
}





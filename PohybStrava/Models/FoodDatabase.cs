using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PohybStrava.Models
{
    public class FoodDatabase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int FoodDatabaseId { get; set; }

        //Navigation properties
        public string UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Potravina")]
        public string FoodItem { get; set; } = "";

        [Display(Name = "Jednotka")]
        public string Unit { get; set; } = "";

        [Display(Name = "Energie (kcal/jednotku) ")]
        public double FoodDatabaseItem { get; set; }

        [Display(Name = "Poznámka")]
        public string Note { get; set; } = "";
    }
}

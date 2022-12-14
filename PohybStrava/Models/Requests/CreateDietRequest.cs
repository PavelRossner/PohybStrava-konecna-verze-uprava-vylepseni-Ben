namespace PohybStrava.Models.Requests
{
    public class CreateDietRequest
    {
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Food { get; set; }
        public double Energy { get; set; }
        public int Amount { get; set; }
        public double Total  { get; set; }
    }
}

namespace FlapKapBackendChallenge.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int AmountAvailable { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int SellerId { get; set; }
    }
}

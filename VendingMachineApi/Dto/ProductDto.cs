namespace FlapKapBackendChallenge.Dto
{
    public class CreateProductRequestDto
    {
        public int Cost { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int AmountAvailable { get; set; } = 0;
    }

    public class CreateProductResponseDto
    {
        public int Id { get; set; }
        public int AmountAvailable { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int SellerId { get; set; }
    }

    public class UpdateProductRequestDto
    {
        public int? Cost { get; set; } 
        public string? ProductName { get; set; }
        public int? AmountAvailable { get; set; } 
        public int? SellerId { get; set; }
    }

    public class UpdateProductResponseDto
    {
        public int Id { get; set; }
        public int Cost { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int AmountAvailable { get; set; } = 0;
        public int SellerId { get; set; }
    }
}

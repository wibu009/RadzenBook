namespace RadzenBook.Client.Models
{
    public class CartItemResponseDTO
    {
        public Guid ProductId { get; set; } 
        public Guid ProductTypeId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
}

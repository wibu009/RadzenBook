namespace RadzenBook.Client.Models
{
    public class OrderItemResponseDTO
    {
        public List<CartItemResponseDTO> Items = new List<CartItemResponseDTO>();
        public decimal ShippingCost = 30000;
    }
}

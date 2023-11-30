namespace RadzenBook.Client.Models
{
    public class CartItem
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductTypeId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}

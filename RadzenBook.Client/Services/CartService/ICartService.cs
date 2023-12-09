namespace RadzenBook.Client.Services.CartService
{
    public interface ICartService
    {
        List<CartItemResponseDTO> GetCartItems();
    }
}

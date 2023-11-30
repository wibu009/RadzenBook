using RadzenBook.Client.Models;

namespace RadzenBook.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartItemResponseDTO>> GetCartProduct();
        Task DeleteItem(Guid productId, Guid productTypeId);
        Task UpdateQuantity(CartItemResponseDTO item);
        //authenticated
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();
    }
}

using RadzenBook.Client.Models;

namespace RadzenBook.Client.Services.CartService
{
    public class CartService : ICartService
    {
        public List<CartItemResponseDTO> GetCartItems()
        {
            List<CartItemResponseDTO> result = new List<CartItemResponseDTO>
            {
                new CartItemResponseDTO
                {
                      ProductId = 1,
                      Title = "Cây Cam Ngọt Của Tôi",
                      ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg.webp",
                      Price = 75600,
                      Quantity = 1
                },
                new CartItemResponseDTO
                {
                      ProductId = 2,
                      Title = "Nhà Giả Kim (Tái Bản 2020)",
                      ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/45/3b/fc/aa81d0a534b45706ae1eee1e344e80d9.jpg.webp",
                      Price = 55300,
                      Quantity = 2
                }
            };
            return result;
        }
    }
}

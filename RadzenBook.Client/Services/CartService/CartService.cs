using RadzenBook.Client.Models;
using System.Net.Http.Json;

namespace RadzenBook.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if ((await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
            {
                await _http.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await _localStorage.SetItemAsync("cart", cart);
            }

            await GetCartItemsCount();
        }

        public async Task DeleteItem(Guid productId, Guid productTypeId)
        {
            if (await IsUserAuthenticated())
            {
                var res = await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }
                var cartItem = cart.FirstOrDefault(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
                cart.Remove(cartItem);

                await _localStorage.SetItemAsync("cart", cart);
            }
        }

        public async Task GetCartItemsCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }

        public async Task<List<CartItemResponseDTO>> GetCartProduct()
        {
            if (await IsUserAuthenticated())
            {
                var res = await _http.GetFromJsonAsync<ServiceResponse<List<CartItemResponseDTO>>>("api/cart");
                return res.Data;
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return new List<CartItemResponseDTO>();
                }
                var result = await _http.PostAsJsonAsync("api/cart/products", cart);
                var cartProduct = await result.Content.ReadFromJsonAsync<ServiceResponse<List<CartItemResponseDTO>>>();
                return cartProduct.Data;
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart != null)
            {
                await _http.PostAsJsonAsync("api/cart/store-product", localCart);
            }

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartItemResponseDTO item)
        {
            if (await IsUserAuthenticated())
            {
                var req = new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductTypeId = item.ProductTypeId
                };
                await _http.PutAsJsonAsync("api/cart/update-quantity", req);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }
                var cartItem = cart.FirstOrDefault(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId);
                cartItem.Quantity = item.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }

            OnChange.Invoke();
        }
        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}

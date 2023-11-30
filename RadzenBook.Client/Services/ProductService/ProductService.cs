using RadzenBook.Client.Models;
using System.Net.Http.Json;

namespace RadzenBook.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public event Action ProductsChanged;

        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = (categoryUrl == null) ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryUrl}");
            if (result != null && result.Data != null && result.Data.Count != 0)
            {
                Products = result.Data;
                CurrentPage = 1;
                PageCount = 0;
            }
            else
            {
                Message = "Không tìm thấy sản phẩm";
            }

            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{productId}");
            return result;
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductReponseDTO>>($"api/Product/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            else
            {
                Message = "Không tìm thấy sản phẩm";
            }

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }
    }
}

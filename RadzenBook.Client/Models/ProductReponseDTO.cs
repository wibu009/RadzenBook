using RadzenBook.Client.Pages;

namespace RadzenBook.Client.Models
{
    public class ProductReponseDTO
    {
        public List<Product> Products { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

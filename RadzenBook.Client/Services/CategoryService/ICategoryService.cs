using RadzenBook.Client.Models;

namespace RadzenBook.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
    }
}

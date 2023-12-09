namespace RadzenBook.Client.Services.BookService
{
    public class BookService : IBookService
    {
        public BookService()
        {

        }
        public List<Book> GetFeaturedBooks()
        {
            List<Book> result = new List<Book>
            {
                new Book
                {
                      Id = 1,
                      Title = "Cây Cam Ngọt Của Tôi",
                      ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg.webp",
                      Price = 75600
                },
                new Book
                {
                      Id = 2,
                      Title = "Nhà Giả Kim (Tái Bản 2020)",
                      ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/45/3b/fc/aa81d0a534b45706ae1eee1e344e80d9.jpg.webp",
                      Price = 55300
                },
                new Book
                {
                      Id = 3,
                      Title = "Dark Nhân Tâm",
                      ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/6a/da/bb/185d27fe442a1668cf0196c1b82c87eb.jpg.webp",
                      Price = 56300
                },
                new Book
                {
                     Id = 4,
                     Title = "Rừng Nauy",
                     ImageUrl = "https://salt.tikicdn.com/cache/750x750/ts/product/65/1f/41/28eeabed8bb8877effcd00448e775382.jpg.webp",
                     Price = 105000
                }
            };
            return result;
        }
    }
}

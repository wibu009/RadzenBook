namespace RadzenBook.Application.Catalog.Product.Book;

public class BookProfile : ProductProfile<BookDto>
{
    public BookProfile()
    {
        CreateMap<Domain.Catalog.Book, BookDto>()
            .ReverseMap();
        CreateMap<CreateBookRequest, Domain.Catalog.Book>();
        CreateMap<UpdateBookRequest, Domain.Catalog.Book>();
    }
}
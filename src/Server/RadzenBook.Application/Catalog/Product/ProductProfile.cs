using RadzenBook.Application.Catalog.Product.Book;

namespace RadzenBook.Application.Catalog.Product;

public class ProductProfile<TProductDto> : Profile
    where TProductDto : ProductDto
{
    protected ProductProfile()
    {
        CreateMap<Domain.Catalog.Product, TProductDto>();
        CreateMap<TProductDto, Domain.Catalog.Product>();
    }
}
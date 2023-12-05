namespace RadzenBook.Application.Catalog.ProductImage;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        CreateMap<Domain.Catalog.ProductImage, ProductImageDto>();
    }
}
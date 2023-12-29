using RadzenBook.Application.Catalog.Product.Book;

namespace RadzenBook.Application.Catalog.Product;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        #region Book

        CreateMap<CreateBookRequest, Domain.Catalog.Product>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Book, opt => opt.MapFrom(src => src))
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
            .ForMember(x => x.Images, opt => opt.Ignore())
            .ForMember(x => x.ModifiedAt, opt => opt.Ignore());
        CreateMap<CreateBookRequest, Domain.Catalog.Book>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Product, opt => opt.Ignore())
            .ForMember(x => x.Genres, opt => opt.Ignore())
            .ForMember(x => x.Author, opt => opt.Ignore())
            .ForMember(x => x.Publisher, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
            .ForMember(x => x.ModifiedAt, opt => opt.Ignore());
        CreateMap<UpdateBookRequest, Domain.Catalog.Product>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Book, opt => opt.MapFrom(src => src))
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
            .ForMember(x => x.ModifiedAt, opt => opt.Ignore())
            .ForMember(x => x.Images, opt => opt.Ignore());

        #endregion

        CreateMap<ProductImage, ProductImageDto>();
    }
}
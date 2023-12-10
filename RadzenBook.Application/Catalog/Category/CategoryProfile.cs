namespace RadzenBook.Application.Catalog.Category;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Domain.Catalog.Category, CategoryDto>()
            .ForMember(d => d.TotalProducts, opt => opt.MapFrom(s => s.Products.Count));
        CreateMap<CreateCategoryRequest, Domain.Catalog.Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)));
        CreateMap<UpdateCategoryRequest, Domain.Catalog.Category>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)));
    }
}
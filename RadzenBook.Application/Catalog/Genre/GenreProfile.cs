namespace RadzenBook.Application.Catalog.Genre;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Domain.Catalog.Genre, GenreDto>()
            .ForMember(d => d.TotalBooks, opt => opt.MapFrom(s => s.Books.Count));
        CreateMap<CreateGenreRequest, Domain.Catalog.Genre>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)));
        CreateMap<UpdateGenreRequest, Domain.Catalog.Genre>()
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)));
    }
}
namespace RadzenBook.Application.Catalog.Author;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Domain.Catalog.Author, AuthorDto>();
        CreateMap<CreateAuthorRequest, Domain.Catalog.Author>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.FullName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Alias, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Alias)))
            .ForMember(dest => dest.Biography, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Biography)));
        CreateMap<UpdateAuthorRequest, Domain.Catalog.Author>()
            .ForMember(dest => dest.FullName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Alias, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Alias)))
            .ForMember(dest => dest.Biography, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Biography)))
            .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth.HasValue))
            .ForMember(dest => dest.DateOfDeath, opt => opt.Condition(src => src.DateOfDeath.HasValue));
    }
}
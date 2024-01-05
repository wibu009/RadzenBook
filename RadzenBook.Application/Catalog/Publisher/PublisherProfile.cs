namespace RadzenBook.Application.Catalog.Publisher;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Domain.Catalog.Publisher, PublisherDto>();
        CreateMap<CreatePublisherRequest, Domain.Catalog.Publisher>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
            .ForMember(dest => dest.Email, opt => opt.Condition(src =>!string.IsNullOrEmpty(src.Email)));
        CreateMap<UpdatePublisherRequest, Domain.Catalog.Publisher>()
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
            .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)));

    }
}

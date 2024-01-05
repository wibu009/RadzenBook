namespace RadzenBook.Application.Catalog.PublisherAddress;

public class GetPublisherAddressProfile : Profile
{
	public GetPublisherAddressProfile()
	{
        CreateMap<Domain.Catalog.PublisherAddress, PublisherAddressDto>();
        CreateMap<CreatePublisherAddressRequest, Domain.Catalog.PublisherAddress>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))          
            .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AddressLine1)))
            .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AddressLine2)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.City)))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.State)))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Country)))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ZipCode)))
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType));

        CreateMap<UpdatePublisherAddressRequest, Domain.Catalog.PublisherAddress>()
            .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AddressLine1)))
            .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AddressLine2)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.City)))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.State)))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Country)))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ZipCode)))
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType));
    }
}

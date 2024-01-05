using RadzenBook.Application.Catalog.Author;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.CustomerAddress;

public class CustomerAddressProfile : Profile
{
    public CustomerAddressProfile()
    {
        CreateMap<Domain.Catalog.CustomerAddress, CustomerAddressDto>();
        CreateMap<CreateCustomerAddressRequest, Domain.Catalog.CustomerAddress>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ConsigneeName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.ConsigneePhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneePhoneNumber)))
            .ForMember(dest => dest.AddressLine1, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.AddressLine2, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.City, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.State, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.Country, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Country)))
            .ForMember(dest => dest.ZipCode, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ZipCode)));
        CreateMap<UpdateCustomerAddressRequest, Domain.Catalog.CustomerAddress>()
           .ForMember(dest => dest.ConsigneeName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.ConsigneePhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneePhoneNumber)))
            .ForMember(dest => dest.AddressLine1, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.AddressLine2, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.City, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.State, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ConsigneeName)))
            .ForMember(dest => dest.Country, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Country)))
            .ForMember(dest => dest.ZipCode, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ZipCode)));
    }
}
using RadzenBook.Application.Catalog.Address.Command;
using RadzenBook.Application.Catalog.Address.Query;

namespace RadzenBook.Application.Catalog.Address;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Domain.Catalog.Address, AddressDto>();
        CreateMap<CreateAddressRequest, Domain.Catalog.Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.AppUserId,
                opt => opt.Condition(src => string.IsNullOrEmpty(src.AppUserId.ToString())))
            .ForMember(dest => dest.Email, opt => opt.Condition(src => string.IsNullOrEmpty(src.Email)));
    }
}
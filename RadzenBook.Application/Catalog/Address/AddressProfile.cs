using RadzenBook.Application.Catalog.Address.Query;

namespace RadzenBook.Application.Catalog.Address;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Domain.Catalog.Address, AddressDto>().ReverseMap();
    }
}
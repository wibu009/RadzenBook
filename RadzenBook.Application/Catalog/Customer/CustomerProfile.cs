using RadzenBook.Application.Catalog.Author;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Customer;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Domain.Catalog.Customer, CustomerDto>();
        CreateMap<CreateCustomerRequest, Domain.Catalog.Customer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FirstName)))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.LastName)))
            .ForMember(dest => dest.Gender, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
            .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth.HasValue))
            .ForMember(dest => dest.UserId, opt => opt.Condition(src => src.UserId.HasValue))
            .ForMember(dest => dest.CartId, opt => opt.Condition(src => src.CartId.HasValue));
        CreateMap<UpdateCustomerRequest, Domain.Catalog.Customer>()           
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FirstName)))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.LastName)))
            .ForMember(dest => dest.Gender, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
            .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth.HasValue));

    }
}
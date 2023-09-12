using AutoMapper;
using RadzenBook.Common.Enums;
using RadzenBook.Contract.DTO.Demo;
using RadzenBook.Entity;

namespace RadzenBook.Contract.Mappers;

public class DemoProfile : Profile
{
    public DemoProfile()
    {
        CreateMap<Demo, DemoDto>()
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => src.DemoEnum.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLocalTime()))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt.ToLocalTime()));
        CreateMap<DemoCreateDto, Demo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => Enum.Parse<DemoEnum>(src.DemoEnum)));
        CreateMap<DemoUpdateDto, Demo>()
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => Enum.Parse<DemoEnum>(src.DemoEnum)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest =>dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)));
    }
}
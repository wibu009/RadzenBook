﻿using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Demo;

public class DemoProfile : Profile
{
    public DemoProfile()
    {
        CreateMap<Domain.Catalog.Demo, DemoDto>()
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => src.DemoEnum.ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLocalTime()))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt.ToLocalTime()));
        CreateMap<CreateDemoRequest, Domain.Catalog.Demo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => Enum.Parse<DemoEnum>(src.DemoEnum)));
        CreateMap<UpdateDemoRequest, Domain.Catalog.Demo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DemoEnum, opt => opt.MapFrom(src => Enum.Parse<DemoEnum>(src.DemoEnum)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)));
    }
}
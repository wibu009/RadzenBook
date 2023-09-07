using AutoMapper;
using RadzenBook.Contract.DTO;
using RadzenBook.Entity;

namespace RadzenBook.Contract.Mapper;

public class DemoProfile : Profile
{
    public DemoProfile()
    {
        CreateMap<Demo, DemoDto>().ReverseMap();
        CreateMap<Demo, DemoInputDto>().ReverseMap();
    }
}
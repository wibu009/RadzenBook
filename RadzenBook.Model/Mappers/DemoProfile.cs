using AutoMapper;
using FirstBlazorProject_BookStore.Entity;
using FirstBlazorProject_BookStore.Model.DTOs;

namespace FirstBlazorProject_BookStore.Model.Mappers;

public class DemoProfile : Profile
{
    public DemoProfile()
    {
        CreateMap<Demo, DemoDto>().ReverseMap();
        CreateMap<Demo, DemoInputDto>().ReverseMap();
    }
}
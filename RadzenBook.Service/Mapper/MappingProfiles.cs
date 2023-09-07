using AutoMapper;
using FirstBlazorProject_BookStore.Model.DTOs.Demo;

namespace FirstBlazorProject_BookStore.Service.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles ()
    {
        #region Demo
        CreateMap<Entity.Demo, DemoDto>();
        CreateMap<DemoInputDto, Entity.Demo>();
        #endregion
    }
}
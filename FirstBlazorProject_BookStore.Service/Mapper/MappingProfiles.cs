using AutoMapper;
using FirstBlazorProject_BookStore.Common.DTO.Demo;

namespace FirstBlazorProject_BookStore.Service.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles ()
    {
        #region Demo
        CreateMap<DataAccess.Entities.Demo, DemoDto>();
        CreateMap<DemoInputDto, DataAccess.Entities.Demo>();
        #endregion
    }
}
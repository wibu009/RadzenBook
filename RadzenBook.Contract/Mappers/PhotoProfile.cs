using AutoMapper;
using RadzenBook.Contract.DTO.Photo;
using RadzenBook.Entity;

namespace RadzenBook.Contract.Mappers;

public class PhotoProfile : Profile
{
    public PhotoProfile()
    {
        CreateMap<Photo, PhotoDto>();
    }
}
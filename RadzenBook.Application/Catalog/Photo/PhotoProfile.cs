namespace RadzenBook.Application.Catalog.Photo;

public class PhotoProfile : Profile
{
    public PhotoProfile()
    {
        CreateMap<Domain.Entities.Photo, PhotoDto>();
    }
}
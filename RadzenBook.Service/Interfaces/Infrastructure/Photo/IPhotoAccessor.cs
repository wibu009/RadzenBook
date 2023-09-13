using Microsoft.AspNetCore.Http;
using RadzenBook.Contract.DTO.Photo;

namespace RadzenBook.Service.Interfaces.Infrastructure.Photo;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task<string> DeletePhotoAsync(string publicId);
}
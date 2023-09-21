using Microsoft.AspNetCore.Http;

namespace RadzenBook.Application.Common.Photo;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task<string> DeletePhotoAsync(string publicId);
}
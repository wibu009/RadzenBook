namespace RadzenBook.Application.Common.Photo;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task<PhotoUploadResult> UpdatePhotoAsync(IFormFile file, string publicId);
    Task<string> DeletePhotoAsync(string publicId);
}
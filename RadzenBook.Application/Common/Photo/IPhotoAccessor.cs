namespace RadzenBook.Application.Common.Photo;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task<PhotoUploadResult> UpdatePhotoAsync(IFormFile file, string publicId);
    Task<PhotoUploadResult> UpdatePhotoByUrlAsync(IFormFile file, string url);

    Task<string> DeletePhotoAsync(string publicId);
    Task<string> DeletePhotoByUrlAsync(string url);
}
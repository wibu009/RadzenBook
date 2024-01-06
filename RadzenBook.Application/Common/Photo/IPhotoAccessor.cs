using Microsoft.AspNetCore.Components.Forms;

namespace RadzenBook.Application.Common.Photo;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    Task<PhotoUploadResult> AddPhotoAsync(IBrowserFile file);
    Task<List<PhotoUploadResult>> AddRangePhotoAsync(ICollection<IFormFile> files);
    Task<List<PhotoUploadResult>> AddRangePhotoAsync(ICollection<IBrowserFile> files);
    Task<PhotoUploadResult> UpdatePhotoAsync(IFormFile file, string publicId);
    Task<string> DeletePhotoAsync(string publicId);
    Task<List<string>> DeleteRangePhotoAsync(ICollection<string> publicIdsOrUrls);
}
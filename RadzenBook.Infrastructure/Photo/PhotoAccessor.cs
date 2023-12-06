using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RadzenBook.Application.Common.Photo;
using RadzenBook.Infrastructure.Common.Extensions;

namespace RadzenBook.Infrastructure.Photo;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    private readonly CloudinarySettings _cloudinarySettings;

    public PhotoAccessor(IConfiguration config)
    {
        _cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>()!;

        var account = new Account(_cloudinarySettings!.CloudName, _cloudinarySettings.ApiKey,
            _cloudinarySettings.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) return null!;
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = _cloudinarySettings.Folder,
            Transformation = new Transformation().Crop("scale").Gravity("face")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new Exception(uploadResult.Error.Message);
        }

        return new PhotoUploadResult
        {
            PublicId = uploadResult.PublicId,
            Url = uploadResult.SecureUrl.ToString()
        };
    }

    public async Task<string> DeletePhotoAsync(string publicIdOrUrl)
    {
        var publicId = publicIdOrUrl.IsUrl()
            ? GetPublicIdFromUrl(publicIdOrUrl)
            : publicIdOrUrl;
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok" ? result.Result : null!;
    }

    public async Task<PhotoUploadResult> UpdatePhotoAsync(IFormFile file, string publicIdOrUrl)
    {
        var publicId = publicIdOrUrl.IsUrl()
            ? GetPublicIdFromUrl(publicIdOrUrl)
            : publicIdOrUrl;
        await DeletePhotoAsync(publicId);
        return await AddPhotoAsync(file);
    }

    private string GetPublicIdFromUrl(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var lastSegment = segments.Last();
        var publicId = _cloudinarySettings.Folder + '/' + Path.GetFileNameWithoutExtension(lastSegment);
        return publicId;
    }
}
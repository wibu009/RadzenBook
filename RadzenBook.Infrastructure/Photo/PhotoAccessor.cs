using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Infrastructure.Photo;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    private const string CloudFolder = "RadzenStore";

    public PhotoAccessor(IConfiguration config)
    {
        var cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>();

        var account = new Account(cloudinarySettings!.CloudName, cloudinarySettings.ApiKey,
            cloudinarySettings.ApiSecret);

        _cloudinary = new Cloudinary(account);
    }

    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) return null!;
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = CloudFolder,
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

    public async Task<string> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok" ? result.Result : null!;
    }

    public async Task<PhotoUploadResult> UpdatePhotoAsync(IFormFile file, string publicId)
    {
        await DeletePhotoAsync(publicId);
        return await AddPhotoAsync(file);
    }

    public async Task<PhotoUploadResult> UpdatePhotoByUrlAsync(IFormFile file, string url)
    {
        var publicId = GetPublicIdFromUrl(url);
        return await UpdatePhotoAsync(file, publicId);
    }

    public async Task<string> DeletePhotoByUrlAsync(string url)
    {
        var publicId = GetPublicIdFromUrl(url);
        return await DeletePhotoAsync(publicId);
    }

    private static string GetPublicIdFromUrl(string url)
    {
        var uri = new Uri(url);
        var segments = uri.Segments;
        var lastSegment = segments.Last();
        var publicId = CloudFolder + '/' + Path.GetFileNameWithoutExtension(lastSegment);
        return publicId;
    }
}
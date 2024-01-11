using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Components.Forms;
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

    //add IBrowserFile support
    public async Task<PhotoUploadResult> AddPhotoAsync(IBrowserFile file)
    {
        if (file.Size <= 0) return null!;
        var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.Name, stream),
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

    public async Task<List<PhotoUploadResult>> AddRangePhotoAsync(ICollection<IFormFile> files)
    {
        var photoUploadResults = new List<PhotoUploadResult>();
        if (!files.Any()) return photoUploadResults;
        foreach (var file in files)
        {
            var photoUploadResult = await AddPhotoAsync(file);
            photoUploadResults.Add(photoUploadResult);
        }

        return photoUploadResults;
    }

    public async Task<List<PhotoUploadResult>> AddRangePhotoAsync(ICollection<IBrowserFile> files)
    {
        var photoUploadResults = new List<PhotoUploadResult>();
        if (!files.Any()) return photoUploadResults;
        foreach (var file in files)
        {
            var photoUploadResult = await AddPhotoAsync(file);
            photoUploadResults.Add(photoUploadResult);
        }

        return photoUploadResults;
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

    //delete range of photos
    public async Task<List<string>> DeleteRangePhotoAsync(ICollection<string> publicIdsOrUrls)
    {
        var results = new List<string>();
        foreach (var publicIdOrUrl in publicIdsOrUrls)
        {
            var result = await DeletePhotoAsync(publicIdOrUrl);
            results.Add(result);
        }

        return results;
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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Infrastructure.Photo;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoAccessor(IConfiguration config)
    {
        var cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>();

        var account = new Account(cloudinarySettings!.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
        
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) return null!;
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
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
}
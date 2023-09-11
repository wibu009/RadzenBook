using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using RadzenBook.Common.Settings;
using RadzenBook.Service.Interfaces.Infrastructure.Photo;

namespace RadzenBook.Service.Implements.Infrastructure.Photo;

public class PhotoAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoAccessor(IConfiguration config)
    {
        var cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>();

        var account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
        
        _cloudinary = new Cloudinary(account);
    }
}
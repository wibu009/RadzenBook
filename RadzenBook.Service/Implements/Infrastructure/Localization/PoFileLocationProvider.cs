using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using OrchardCore.Localization;

namespace RadzenBook.Service.Implements.Infrastructure.Localization;

public class PoFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    
    public PoFileLocationProvider(IHostEnvironment hostingEnvironment)
    {
        _fileProvider = hostingEnvironment.ContentRootFileProvider;
    }
    
    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        // Loads all *.po files from the culture folder under the Resource Path.
        // For example: /Resources/Localization/en-US/*.po
        foreach (var file in _fileProvider.GetDirectoryContents(Path.Combine("Localization", cultureName)))
        {
            yield return file;
        }
    }
}
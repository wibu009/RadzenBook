using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;

namespace RadzenBook.Infrastructure.Localization;

public class PoFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    private readonly string _resourcesContainer;

    public PoFileLocationProvider(IHostEnvironment hostingEnvironment,
        IOptions<LocalizationOptions> localizationOptions)
    {
        _fileProvider = hostingEnvironment.ContentRootFileProvider;
        _resourcesContainer = localizationOptions.Value.ResourcesPath;
    }

    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        // Loads all *.po files from the culture folder under the Resource Path.
        // For example: /Localization/en-US/*.po
        var result = _fileProvider.GetDirectoryContents(Path.Combine(_resourcesContainer, cultureName));
        return result.Where(x => x.Name.EndsWith(".po"));
    }
}
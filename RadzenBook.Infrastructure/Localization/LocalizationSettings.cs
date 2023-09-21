namespace RadzenBook.Infrastructure.Localization;

public class LocalizationSettings
{
    public string[]? SupportedCultures { get; set; } = default!;
    public string DefaultRequestCulture { get; set; } = string.Empty;
    public string ResourcesPath { get; set; } = string.Empty;
    public string ResourcesType { get; set; } = string.Empty;
    public bool EnableLocalization { get; set; } = false;
    public bool FallbackToParent { get; set; } = false;
}

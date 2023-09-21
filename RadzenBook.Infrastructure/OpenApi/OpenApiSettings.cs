namespace RadzenBook.Infrastructure.OpenApi;

public class OpenApiSettings
{
    public Info Info { get; set; } = new Info();
    public Security Security { get; set; } = new Security();
}

public class Info
{
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Contact Contact { get; set; } = new Contact();
    public License License { get; set; } = new License();
    public string TermsOfService { get; set; } = string.Empty;
}

public class Contact
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class License
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class Security
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string In { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Scheme { get; set; } = string.Empty;
    public string BearerFormat { get; set; } = string.Empty;
}
using System.Web;

namespace RadzenBook.Infrastructure.Common.Extensions;

public static class UrlExtension
{
    public static string GetBaseUrl(this HttpRequest request)
    {
        var scheme = request.Scheme;
        var host = request.Host;
        var pathBase = request.PathBase;

        return $"{scheme}://{host}{pathBase}";
    }

    public static string AddQueryParam(this string url, string key, string value)
    {
        if (url == null)
            throw new ArgumentNullException(nameof(url));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key is null or empty", nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var qs = $"{key}={Uri.EscapeDataString(value)}";

        return url.Contains('?') ? $"{url}&{qs}" : $"{url}?{qs}";
    }

    public static Dictionary<string, string> DecodeQueryStringToDict(this string queryString)
    {
        var parsed = HttpUtility.ParseQueryString(queryString);

        return parsed.AllKeys.ToDictionary(key => key, key => HttpUtility.UrlDecode(parsed[key]))!;
    }

    public static bool IsUrl(this string potentialUrl)
    {
        if (Uri.TryCreate(potentialUrl, UriKind.Absolute, out var uriResult))
        {
            return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        return false;
    }
}
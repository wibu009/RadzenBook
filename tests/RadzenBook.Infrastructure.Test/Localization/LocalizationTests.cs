using System.Globalization;
using Microsoft.Extensions.Localization;

namespace RadzenBook.Infrastructure.Test.Localization;

public class LocalizationTests
{
    private const string TestString1 = "test string 1";
    private const string TestString1InEnglish = "test string 1 in english";
    private const string TestString1InVietnamese = "kiểm tra chuỗi 1 trong tiếng việt";
    private const string TestString2 = "test string 2";
    private const string TestString2InEnglish = "test string 2 in english";

    private readonly IStringLocalizer _localizer;

    public LocalizationTests(IStringLocalizer<LocalizationTests> localizer) => _localizer = localizer;

    [Theory]
    [InlineData("ja-JP", TestString1, TestString1)]
    [InlineData("en-US", TestString1, TestString1InEnglish)]
    [InlineData("vi-VN", TestString1, TestString1InVietnamese)]
    [InlineData("en-US", TestString2, TestString2InEnglish)]
    [InlineData("vi-VN", TestString2, TestString2)]
    public void TranslateToCultureTest(string culture, string testString, string translatedString)
    {
        Thread.CurrentThread.CurrentCulture =
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);

        var result = _localizer[testString];

        Assert.Equal(translatedString, result);
    }
}
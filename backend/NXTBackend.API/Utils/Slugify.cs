using System.Text;
using System.Text.RegularExpressions;

namespace NXTBackend.API.Utils;

public static class StringExtensions
{
    public static string ToUrlSlug(this string value)
    {
        value = value.ToLowerInvariant();
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
        value = value.Trim('-', '_');
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
        return value;
    }
}

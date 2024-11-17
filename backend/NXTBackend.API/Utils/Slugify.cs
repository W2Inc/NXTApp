using System.Text;
using System.Text.RegularExpressions;

namespace NXTBackend.API.Utils;

public static class StringExtensions
{
    public static string ToUrlSlug(this string value)
    {
        //First to lower case
        value = value.ToLowerInvariant();
        //Replace spaces
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
        //Remove invalid chars
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
        //Trim dashes from end
        value = value.Trim('-', '_');
        //Replace double occurrences of - or _
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
        return value;
    }
}

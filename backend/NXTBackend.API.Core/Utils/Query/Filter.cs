// ============================================================================
// W2Inc, Amsterdam 2023-2025, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

namespace NXTBackend.API.Core.Utils.Query;

public class FilterDictionary : Dictionary<string, object?>
{
    public FilterDictionary AddFilter(string key, object? value)
    {
        if (value is not null)
            this[key.ToLowerInvariant()] = value;
        return this;
    }
}

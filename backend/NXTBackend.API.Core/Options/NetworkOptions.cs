// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.Extensions.Configuration;

namespace NXTBackend.API.Core.Options;

// ============================================================================

public class NetworkOptions
{
    public required IEnumerable<string> AllowedIpRanges { get; set; }
}

// ============================================================================

public static partial class ConfigurationExtensions
{
    public static T? GetNetworkOptions<T>(this IConfiguration configuration, string configSectionName = "NetworkSettings") where T : GitRemoteOptions
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.GetSection(configSectionName).Get<T>();
    }
}

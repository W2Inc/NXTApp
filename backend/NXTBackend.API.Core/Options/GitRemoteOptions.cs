// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.Extensions.Configuration;

namespace NXTBackend.API.Core.Options;

// ============================================================================

public class GitRemoteOptions
{
    public string? DefaultBranch { get; set; } = "main";
    public required string ApiUrl { get; set; }
    public required string Template { get; set; }
}

// ============================================================================

public static partial class ConfigurationExtensions
{
    /// <summary>
    /// Retrieves the Keycloak options from the configuration section with the specified name.
    /// </summary>
    /// <typeparam name="T">The type of the Keycloak options.</typeparam>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="configSectionName">The name of the configuration section. Default is ConfigurationConstants.ConfigurationPrefix.</param>
    /// <returns>The Keycloak options instance.</returns>
    public static T? GetGitRemoteOptions<T>(this IConfiguration configuration, string configSectionName = "GitRemote") where T : GitRemoteOptions
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.GetSection(configSectionName).Get<T>();
    }
}

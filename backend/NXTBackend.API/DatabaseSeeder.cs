// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Infrastructure.Database;

// ============================================================================

namespace NXTBackend.API;

/// <summary>
/// Class to seed the database with fake data.
/// </summary>
public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly DatabaseContext _databaseContext;

    public DatabaseSeeder(
        ILogger<DatabaseSeeder> logger,
        DatabaseContext databaseContext
    )
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Initializes the database with the necessary data.
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="args">If the --seed param is given, will seed the database with fake data</param>
    /// <returns></returns>
    public async Task<bool> InitializeAsync(IConfiguration configuration, string[] args)
    {
        await _databaseContext.Database.MigrateAsync();
        await _databaseContext.SaveChangesAsync();
        return true;
    }
}

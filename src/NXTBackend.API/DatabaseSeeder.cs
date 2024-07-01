using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly DatabaseContext _databaseContext;

    public DatabaseSeeder(
        ILogger<DatabaseSeeder> logger,
        DatabaseContext databaseContext)
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
        //if (!args.Contains("--seed"))
        //    return true;

        bool hasChanges = false;
        Feature? x = null;
        if ((x = await _databaseContext.Features.FirstOrDefaultAsync(x => x.Name == "Feature 1")) == null)
        {
            await _databaseContext.Features.AddAsync(x = new()
            {
                Name = "Feature 1",
            });

            hasChanges = true;
        }

        User? y = null;
        if ((y = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Login == "w2wizard")) == null)
        {
            var dets = await _databaseContext.Details.AddAsync(new()
            {

            });
            await _databaseContext.Users.AddAsync(y = new()
            {
                Login = "w2wizard",
                DetailsId = dets.Entity.Id
            });

            hasChanges = true;
        }

        //Details? z = null;
        //if ((z = await _databaseContext.Details.FirstOrDefaultAsync(x => x.Login == "w2wizard")) == null)
        //{
        //    await _databaseContext.Users.AddAsync(y = new()
        //    {
        //        Login = "w2wizard",
        //    });

        //    hasChanges = true;
        //}

        _logger.LogInformation("Feature 1: {0}", x.Name);

        //x.Name = "Feature 1";
        //_databaseContext.Features.Update(x);
        hasChanges = true;

        if (hasChanges)
            await _databaseContext.SaveChangesAsync();
        return true;
    }
}
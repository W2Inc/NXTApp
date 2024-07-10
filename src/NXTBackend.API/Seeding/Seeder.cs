using Microsoft.AspNetCore.Hosting.Server;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using Serilog;

namespace NXTBackend.API.Seeding;

public interface IGenerator<T>
{
    /// <summary>
    /// Generate a range of entities.
    /// </summary>
    /// <param name="count">How many.</param>
    /// <returns> The generated entities.</returns>
    public IEnumerable<T> GenerateEntities(int count) => throw new NotImplementedException();

    /// <summary>
    /// Generate a single entity.
    /// </summary>
    /// <returns> The generated entity.</returns>
    public T Generate() => GenerateEntities(1).First();
}

/// <summary>
/// Base class for seeders.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseSeeder<T> where T : BaseEntity
{
    protected readonly DatabaseContext _databaseContext;

    protected BaseSeeder(DatabaseContext DatabaseContext)
    {
        _databaseContext = DatabaseContext;
    }

    /// <summary>
    /// Seed the database with fake data.
    /// </summary>
    /// <returns></returns>
    public abstract Task SeedAsync();

    public IEnumerable<T> GenerateEntities(int count) => throw new NotImplementedException();

    /// <summary>
    /// Generate a single entity.
    /// </summary>
    /// <returns> The generated entity.</returns>
    public T Generate() => GenerateEntities(1).First();
}

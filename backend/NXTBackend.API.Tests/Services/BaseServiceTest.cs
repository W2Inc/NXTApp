using NXTBackend.API.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Bogus;

namespace NXTBackend.API.Tests;

public abstract class BaseServiceTest : IDisposable
{
    protected readonly TestFixture Fixture;
    protected readonly DatabaseContext DbContext;
    protected readonly ServiceProvider ServiceProvider;
    protected readonly Faker Faker;

    protected BaseServiceTest()
    {
        Faker = new Faker();
        Fixture = new TestFixture();
        DbContext = Fixture.DbContext;
        ServiceProvider = Fixture.ServiceProvider;
    }

    public void Dispose()
    {
        Fixture.Dispose();
    }
}

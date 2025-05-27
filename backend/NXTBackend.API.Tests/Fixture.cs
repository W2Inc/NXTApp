using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NXTBackend.API.Core.Services.Implementation;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Infrastructure.Database;
using System;

namespace NXTBackend.API.Tests
{
    public class TestFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }
        public DatabaseContext DbContext { get; }

        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DatabaseContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton(TimeProvider.System);
            RegisterServices(services);

            ServiceProvider = services.BuildServiceProvider();
            DbContext = ServiceProvider.GetRequiredService<DatabaseContext>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            // Register all your services
            services.AddScoped<ICursusService, CursusService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserCursusService, UserCursusService>();
            services.AddScoped<IUserGoalService, UserGoalService>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IGoalService, GoalService>();
            // TODO: Projects are not *really* testible due to the external git requirement...
            // services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IRubricService, RubricService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IResourceOwnerService, ResourceOwnerService>();
            services.AddScoped<ISpotlightEventService, SpotlightEventService>();
            services.AddScoped<ISpotlightEventActionService, SpotlightEventActionService>();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
            ServiceProvider.Dispose();
        }
    }
}

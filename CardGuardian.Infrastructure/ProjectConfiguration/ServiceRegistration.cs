using CardGuardian.Application.Services;
using CardGuardian.Domain.Entities;
using CardGuardian.Infrastructure.Persistance;
using CardGuardian.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CardGuardian.Infrastructure.ProjectConfiguration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

            serviceCollection.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            serviceCollection.AddScoped<ICountryService, CountryRepository>();

            return serviceCollection;
        }
    }
}

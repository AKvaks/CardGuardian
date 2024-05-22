using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CardGuardian.Application.ProjectConfiguration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return servicesCollection;
        }
    }
}

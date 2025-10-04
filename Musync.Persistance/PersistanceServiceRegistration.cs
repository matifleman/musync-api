using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MusyncDbContext>(
                options => options.UseSqlite(configuration.GetConnectionString("MusyncDatabaseConnectionString"))
            );

            return services;
        }

    }
}

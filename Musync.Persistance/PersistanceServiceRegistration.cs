using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musync.Persistance.DatabaseContext;
using Musync.Domain;
using Musync.Application.Contracts.Persistance;
using Musync.Persistance.Repositories;

namespace Musync.Persistance
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MusyncDbContext>(
                options => options.UseSqlite(configuration.GetConnectionString("MusyncDatabaseConnectionString"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<MusyncDbContext>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IInstrumentRepository, InstrumentRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();

            return services;
        }

    }
}

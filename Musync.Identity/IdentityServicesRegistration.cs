using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Musync.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Musync.Identity.DatabaseContext;

namespace Musync.Identity
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MusyncIdentityDbContext>(
                options => options.UseSqlite(configuration.GetConnectionString("MusyncDatabaseConnectionString"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<MusyncIdentityDbContext>();

            return services;
        }
    }
}

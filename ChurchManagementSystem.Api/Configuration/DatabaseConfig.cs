
using ChurchManagementSystem.Core.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;

namespace ChurchManagementSystem.API.Configuration
{
    public static class DatabaseConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<DataContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("DataContext"), sql => sql.MigrationsAssembly("ChurchManagementSystem.Core")));
            //opt.UseNpgsql(config.GetConnectionString("DataContext"), sql => sql.UseNetTopologySuite()));

        }

        public static void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                var container = serviceScope.ServiceProvider.GetService<Container>();

                //context.Database.EnsureCreated();
                //context.Database.Migrate();
            }
        }
    }
}
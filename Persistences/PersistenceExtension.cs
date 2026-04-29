using Applications.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistences.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistences
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.GetName().Name);
                    });
            });
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}

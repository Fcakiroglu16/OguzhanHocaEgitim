using Applications;
using Applications.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistences.Repositories;


namespace Persistences
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.GetName().Name);
                    });
            });
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

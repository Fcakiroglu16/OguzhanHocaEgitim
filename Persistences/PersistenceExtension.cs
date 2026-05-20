using Applications;
using Applications.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistences.Histories;
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


            services.AddOptions<MongoOption>().BindConfiguration(nameof(MongoOption));

            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var options = sp.GetRequiredService<MongoOption>();
                return new MongoClient(options.ConnectionString);
            });


            services.AddScoped<MongoDbContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var options = sp.GetRequiredService<MongoOption>();

                return MongoDbContext.Create(mongoClient.GetDatabase(options.DatabaseName));
            });
        }
    }
}

using Applications;
using Applications.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Persistences.Histories;
using Persistences.Interceptors;
using Persistences.Repositories;


namespace Persistences
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseLazyLoadingProxies();
                var mongoDbContext = sp.GetRequiredService<MongoDbContext>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();


                options.AddInterceptors(new SaveChangesInterceptors(
                    mongoDbContext,
                    httpContextAccessor
                ));

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
                var options = sp.GetRequiredService<IOptions<MongoOption>>().Value;
                return new MongoClient(options.ConnectionString);
            });


            services.AddScoped<MongoDbContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var options = sp.GetRequiredService<IOptions<MongoOption>>().Value;

                return MongoDbContext.Create(mongoClient.GetDatabase(options.DatabaseName));
            });
        }
    }
}

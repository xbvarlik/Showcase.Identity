using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Settings;

namespace Showcase.Identity.Data;

public static class Bootstrapper
{
    public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlDb(configuration);
        services.AddMongoDb(configuration);
    }
    
    private static void AddSqlDb(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<MssqlContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString(BootstrapperConstant.MssqlContextName)));

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<MongoDbContext>();
    }
}
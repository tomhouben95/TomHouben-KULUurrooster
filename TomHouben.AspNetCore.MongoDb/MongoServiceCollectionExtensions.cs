using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TomHouben.AspNetCore.MongoDb.Abstractions;

namespace TomHouben.AspNetCore.MongoDb
{
    public static class MongoServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoConnection(this IServiceCollection services, string mongoConnectionString)
        {
            var databaseConnection = new MongoConnection(mongoConnectionString);
            services.AddSingleton<IMongoConnection>(databaseConnection);
            
            return services;
        }
    }
}
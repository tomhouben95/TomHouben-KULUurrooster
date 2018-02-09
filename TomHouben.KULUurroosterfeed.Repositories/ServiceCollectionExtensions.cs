using System;
using Microsoft.Extensions.DependencyInjection;
using TomHouben.KULUurroosterfeed.Repositories.Abstractions;

namespace TomHouben.KULUurroosterfeed.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITimeTableEntryRepository, TimeTableEntryRepository>();

            return services;
        }
    }
}

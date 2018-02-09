using System;
using Microsoft.Extensions.DependencyInjection;
using TomHouben.KULUurroosterfeed.Services.Abstractions;

namespace TomHouben.KULUurroosterfeed.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ITimeTableEntryService, TimeTableEntryService>();

            return services;
        }
    }
}

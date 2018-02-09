using System;
using Microsoft.Extensions.DependencyInjection;
using TomHouben.KULUurroosterfeed.ICalService.Abstractions;

namespace TomHouben.KULUurroosterfeed.ICalService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterICalService(this IServiceCollection services)
        {
            services.AddScoped<IICalService, ICalService>();

            return services;
        }
    }
}

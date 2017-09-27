using System;
using Microsoft.Extensions.DependencyInjection;
using TomHouben.KULUurroosterfeed.HTMLParserServices.Abstractions;

namespace TomHouben.KULUurroosterfeed.HTMLParserServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterHtmlParserServices(this IServiceCollection services)
        {
            services.AddScoped<IEffectiveTimeTableParser, EffectiveTimeTableParser>();

            return services;
        }
    }
}

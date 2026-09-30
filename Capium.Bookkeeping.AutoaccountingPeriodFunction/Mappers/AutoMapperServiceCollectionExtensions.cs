using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Mappers
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Assembly assembly)
        {
            return services.AddAutoMapper(cfg => cfg.AddMaps(assembly), Array.Empty<Type>());
        }
    }
}

using System.Reflection;
using ADT.Api.Validation;
using Mapster;
using MapsterMapper;

namespace ADT.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMapster(this IServiceCollection serviceCollection)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        var applicationAssembly = Assembly.GetExecutingAssembly();
        typeAdapterConfig.Scan(applicationAssembly);
        
        serviceCollection.AddSingleton<IMapper>(_ => new Mapper(typeAdapterConfig));

        return serviceCollection;
    }
}
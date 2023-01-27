using System.Reflection;
using ADT.Api.AddressDataTransformer;
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

    public static IAddressDataTransformingStrategy AddAddressDataTransformingStrategy(this IServiceCollection serviceCollection)
    {
        var strategy = new AddressDataTransformingStrategy();
        serviceCollection.AddSingleton<IAddressDataTransformingStrategy>(_ => strategy);

        return strategy;
    }

    public static IAddressDataTransformingStrategy AddTransformer<T>(this IAddressDataTransformingStrategy strategy) where T : IAddressDataTransformer
    {
        strategy.AddType(typeof(T));

        return strategy;
    }
}
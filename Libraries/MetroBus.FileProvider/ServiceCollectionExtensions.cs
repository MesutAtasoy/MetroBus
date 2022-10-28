using Microsoft.Extensions.DependencyInjection;

namespace MetroBus.FileProvider;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetroBusFileProvider(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMetroBusFileProvider, MetroBusFileProvider>();

        return serviceCollection;
    }
}
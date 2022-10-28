using MetroBus.Logging.Loggers;
using Microsoft.Extensions.DependencyInjection;

namespace MetroBus.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetroBusConsoleLogging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(IMetroBusLogger<>), typeof(MetroBusConsoleLogger<>));

        return serviceCollection;
    }
}
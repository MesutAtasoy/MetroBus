using Application.Common;
using Application.Common.Models;
using MainProcess.ServiceExtensions;
using MetroBus.Abstraction;
using MetroBus.Abstraction.EventHandlers;
using MetroBus.Extensions;
using MetroBus.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging(c => c.AddConsole(opt => opt.LogToStandardErrorThreshold = LogLevel.Debug))
    .AddEventBus()
    .RegisterHandlers()
    .AddMetroBusFileProvider()
    .BuildServiceProvider();

DIResolver.SetProvider(serviceProvider);

Console.WriteLine("Instance Id " + Guid.NewGuid().ToString());

var eventBus = serviceProvider.GetService<IEventBus>();
eventBus?.Subscribe<DataCaptureEvent, IIntegrationEventHandler<DataCaptureEvent>>();

Console.ReadLine();
    
    
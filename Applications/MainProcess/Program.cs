using Application.Common.Models;
using Application.Common.Services;
using MainProcess.ServiceExtensions;
using MetroBus.Abstraction;
using MetroBus.Abstraction.EventHandlers;
using MetroBus.Extensions;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddEventBus()
    .RegisterHandlers()
    .AddSingleton<IFileService, FileService>()
    .BuildServiceProvider();

DIResolver.SetProvider(serviceProvider);

var eventBus = serviceProvider.GetService<IEventBus>();
eventBus?.Subscribe<DataCaptureEvent, IIntegrationEventHandler<DataCaptureEvent>>();
Console.ReadLine();
    
    
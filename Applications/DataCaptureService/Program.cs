// See https://aka.ms/new-console-template for more information

using Application.Common;
using DataCaptureService.Listener;
using DataCaptureService.ServiceExtensions;
using DataCaptureService.Services;
using MetroBus.Extensions;
using MetroBus.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging(c => c.AddConsole(opt => opt.LogToStandardErrorThreshold = LogLevel.Debug))
    .AddEventBus()
    .AddMetroBusFileProvider()
    .AddApplicationSettings(x =>
    {
        x.FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // listening specific folder 
        x.AllowedExtensions = new[] { ".pdf", ".jpg", ".epub" }; // allow just pdf format to transmission.
    })
    .AddSingleton<IListener, LocalFolderListener>()
    .AddScoped<IDataCaptureEventService, DataCaptureEventService>()
    .BuildServiceProvider();

DIResolver.SetProvider(serviceProvider);

var listener = serviceProvider.GetService<IListener>();

await listener?.ListenAsync();

Console.ReadLine();

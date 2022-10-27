// See https://aka.ms/new-console-template for more information

using Application.Common.Models;
using Application.Common.Services;
using DataCaptureService.ServiceExtensions;
using DataCaptureService.Services;
using MetroBus.Extensions;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddEventBus()
    .AddScoped<IDataCaptureEventService, DataCaptureEventService>()
    .AddSingleton<IFileService, FileService>()
    .BuildServiceProvider();

DIResolver.SetProvider(serviceProvider);

var service = serviceProvider.GetService<IDataCaptureEventService>();
var fileService = serviceProvider.GetService<IFileService>();

SendFiles();

void SendFiles()
{
    var filePaths = GetFiles();

    var files = new List<FileModel>();
    
    foreach (var filePath in filePaths)
    {
        var file = fileService?.ReadAllBytes(filePath);
        
        service?.SendMessage(new FileModel
        {
            Body = file,
            Name = fileService.GetFileName(filePath),
            Path = filePath
        }, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }

}

List<string> GetFiles()
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
    var filePaths = Directory.GetFiles(path);

    return filePaths.ToList();
}


Console.ReadLine();

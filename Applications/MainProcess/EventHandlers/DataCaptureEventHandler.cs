using Application.Common.Models;
using MetroBus.Abstraction.EventHandlers;
using MetroBus.FileProvider;
using Microsoft.Extensions.Logging;

namespace MainProcess.EventHandlers;

public class DataCaptureEventHandler : IIntegrationEventHandler<DataCaptureEvent>
{
    private readonly IMetroBusFileProvider _fileService;
    private readonly ILogger<DataCaptureEventHandler> _logger;

    public DataCaptureEventHandler(IMetroBusFileProvider fileService, 
        ILogger<DataCaptureEventHandler> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }
    
    public Task Handle(DataCaptureEvent @event)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var folderName = Path.Combine(path, @event.CorrelationId);
        
        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }
        
        _fileService?.SaveFile( @event.MessageBody, Path.Combine(folderName,  $"{@event.Position}"));
        
        if (IsCompletedFile(folderName, @event.Size))
        {
            _fileService?.CombineFiles(folderName, Path.Combine(path, @event.FileName));
            _fileService?.DeleteDirectory(folderName);
        }
        
        _logger.LogInformation($"File processed | {@event.CorrelationId} | {@event.FileName} |  Part {@event.Position} of {@event.Size}");
        
        return Task.CompletedTask;
    }


    private bool IsCompletedFile(string folderName, long size)
    {
        return Directory.GetFiles(folderName).Length == size;
    }
}

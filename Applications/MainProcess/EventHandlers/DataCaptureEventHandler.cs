using Application.Common.Models;
using Application.Common.Services;
using MetroBus.Abstraction.EventHandlers;

namespace MainProcess.EventHandlers;

public class DataCaptureEventHandler : IIntegrationEventHandler<DataCaptureEvent>
{
    private readonly IFileService _fileService;
    
    public DataCaptureEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }
    
    public Task Handle(DataCaptureEvent @event)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var folderName = Path.Combine(path, @event.CorrelationId);
        
        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }
        
        var fileName = $"{@event.CorrelationId}-{@event.Position}"; 
        _fileService?.SaveFile( @event.MessageBody, Path.Combine(folderName, fileName));
        
        if (@event.IsLast)
        {
            var filePaths = Directory.GetFiles(folderName);
            _fileService?.CombineFiles(filePaths, Path.Combine(path, @event.FileName));
            
            _fileService?.DeleteDirectory(folderName);
        }
        
        return Task.CompletedTask;
    }
}

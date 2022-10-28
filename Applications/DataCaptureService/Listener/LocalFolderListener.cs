using Application.Common.Models;
using DataCaptureService.Options;
using DataCaptureService.Services;
using MetroBus.FileProvider;

namespace DataCaptureService.Listener;

public class LocalFolderListener : IListener
{
    private readonly ApplicationOption _applicationOption;
    private readonly IMetroBusFileProvider _fileService;
    private readonly IDataCaptureEventService _service;
    
    public LocalFolderListener(ApplicationOption applicationOption,
        IMetroBusFileProvider fileService, 
        IDataCaptureEventService service)
    {
        _applicationOption = applicationOption;
        _fileService = fileService;
        _service = service;
    }
    
    public async Task ListenAsync()
    {
        var filePaths = Directory.GetFiles(_applicationOption.FolderPath);
        
        foreach (var filePath in filePaths)
        {
            var file = await _fileService?.ReadAllBytes(filePath);

            var extension = new FileInfo(filePath).Extension;
            
            if (!_applicationOption.AllowedExtensions.Contains(extension))
            {
                continue;
            }
        
            _service?.SendMessageAsync(new FileModel
            {
                Body = file,
                Name = _fileService.GetFileName(filePath),
                Path = filePath
            }, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }
    }
}
using Application.Common.Models;
using DataCaptureService.Options;
using MetroBus.Abstraction;
using Microsoft.Extensions.Logging;

namespace DataCaptureService.Listener;

public class LocalFolderListener : IListener
{
    private readonly ApplicationOption _applicationOption;
    private readonly ILogger<LocalFolderListener> _logger;
    private readonly IEventBus _eventBus;
    const int CHUNKSIZE = 1024 * 1024;// 1MB 

    public LocalFolderListener(ApplicationOption applicationOption,
        IEventBus eventBus, 
        ILogger<LocalFolderListener> logger)
    {
        _applicationOption = applicationOption;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task ListenAsync()
    {
        var filePaths = Directory.GetFiles(_applicationOption.FolderPath);

        foreach (var filePath in filePaths)
        {
            var sessionId = Guid.NewGuid().ToString();
            var correlationId = Guid.NewGuid().ToString();
            
            var fileInfo = new FileInfo(filePath);

            if (!_applicationOption.AllowedExtensions.Contains(fileInfo.Extension))
            {
                continue;
            }

            int size = 1;
            
            var countOfArray = fileInfo.Length / CHUNKSIZE;

            if (fileInfo.Length % CHUNKSIZE > 0)
                countOfArray++;

            foreach (var chunk in ReadChunks(filePath))
            {
                var serviceBusMessage = new DataCaptureEvent
                {
                    SessionId = sessionId,
                    CorrelationId = correlationId,
                    Size = countOfArray,
                    MessageBody = chunk,
                    IsLast = countOfArray == size,
                    Position = size,
                    ChunkSize = CHUNKSIZE,
                    FileName = fileInfo.Name
                };
                
                _eventBus.Publish(serviceBusMessage);

                _logger.LogInformation($"Published the {serviceBusMessage.CorrelationId} - {@serviceBusMessage.FileName} -  Part {@serviceBusMessage.Position} of  {@serviceBusMessage.Size}");
                
                size++;
            }
        }
    }
    
    public static IEnumerable<byte[]> ReadChunks(string fileName)
    {

        byte[] filechunk = new byte[CHUNKSIZE];
        int numBytes;
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            long remainBytes = fs.Length;
            int bufferBytes = CHUNKSIZE;

            while (true)
            {
                if (remainBytes <= CHUNKSIZE)
                {
                    filechunk = new byte[remainBytes];
                    bufferBytes = (int)remainBytes;
                }

                if ((numBytes = fs.Read(filechunk, 0, bufferBytes)) > 0)
                {
                    remainBytes -= bufferBytes;

                    yield return filechunk;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
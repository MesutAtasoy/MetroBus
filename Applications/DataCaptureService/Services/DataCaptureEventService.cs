using Application.Common.Models;
using MetroBus.Abstraction;
using Microsoft.Extensions.Logging;

namespace DataCaptureService.Services;

public class DataCaptureEventService : IDataCaptureEventService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<DataCaptureEventService> _logger;

    public DataCaptureEventService(IEventBus eventBus, ILogger<DataCaptureEventService> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task SendMessageAsync(FileModel model, string sessionId, string correlationId)
    {
        var chunkSize = 1024 * 1024;
        int countOfArray = model.Body.Length / chunkSize;

        if (model.Body.Length % chunkSize > 0)
            countOfArray++;

        for (int i = 0; i < countOfArray; i++)
        {
            var body = model.Body.Skip(i * chunkSize).Take(chunkSize).ToArray();

            var serviceBusMessage = new DataCaptureEvent
            {
                SessionId = sessionId,
                CorrelationId = correlationId,
                Size = countOfArray,
                MessageBody = body,
                IsLast = i == countOfArray - 1,
                Position = i + 1,
                ChunkSize = chunkSize,
                FileName = model.Name
            };
            _eventBus.Publish(serviceBusMessage);

            _logger.LogInformation(
                $"Published the {serviceBusMessage.CorrelationId} - {@serviceBusMessage.FileName} -  Part {@serviceBusMessage.Position} of  {@serviceBusMessage.Size}");
        }
    }
}
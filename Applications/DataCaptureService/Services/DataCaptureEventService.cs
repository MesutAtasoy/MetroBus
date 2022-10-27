using System.Text.Json;
using System.Transactions;
using Application.Common.Models;
using MetroBus.Abstraction;

namespace DataCaptureService.Services;

public class DataCaptureEventService : IDataCaptureEventService
{
    private readonly IEventBus _eventBus;
    
    public DataCaptureEventService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public void SendMessage(FileModel model, string sessionId, string correlationId)
    {   
        var sendTasks = new List<Task>();

        var models = new List<DataCaptureEvent>();
        
        var sequenceNo = 0;

        using (var scope = new TransactionScope())
        {
            var chunkSize =  1024 * 1024;

            var expectNo = (double)model.Body.Length / chunkSize;
            var expectedNoMessages = (int)Math.Ceiling(expectNo);

            var bytesToRead = (long) model.Body.Length;
            var nextRead = chunkSize;
            var lastChunk = false;

            byte[] buffer = new byte[chunkSize];

            while (bytesToRead > 0)
            {
                sequenceNo++;

                if (bytesToRead < chunkSize)
                    lastChunk = true;

                if (lastChunk)
                {
                    buffer = new byte[bytesToRead];
                    nextRead = (int)bytesToRead;
                }

                var serviceBusMessageBody = new MemoryStream();
                serviceBusMessageBody.Write(buffer, 0, buffer.Length);
                serviceBusMessageBody.Flush();
                serviceBusMessageBody.Seek(0, SeekOrigin.Begin);
                
                var serviceBusMessage = new DataCaptureEvent
                {
                    SessionId = sessionId,
                    CorrelationId = correlationId,
                    Size = expectedNoMessages,
                    MessageBody = serviceBusMessageBody.ToArray(),
                    IsLast = lastChunk,
                    Position = sequenceNo,
                    ChunkSize = chunkSize,
                    FileName = model.Name
                };

                // sendTasks.Add(Task.Run(()=> _eventBus.Publish(serviceBusMessage)));
                _eventBus.Publish(serviceBusMessage);
                
                models.Add(serviceBusMessage);
                bytesToRead = bytesToRead - nextRead;
            }

            // Task.WaitAll(sendTasks.ToArray());

            
            scope.Complete();
        }
    }
}
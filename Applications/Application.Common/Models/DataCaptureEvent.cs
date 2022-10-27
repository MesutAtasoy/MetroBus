using MetroBus.Abstraction.Events;

namespace Application.Common.Models;

public class DataCaptureEvent : IntegrationEvent
{
    public string CorrelationId { get; set; }
    public string SessionId { get; set; }
    public long Position { get; set; }
    public long Size { get; set; }
    public byte[] MessageBody { get; set; }
    public bool IsLast { get; set; }
    public long ChunkSize { get; set; }
    public string FileName { get; set; }
}
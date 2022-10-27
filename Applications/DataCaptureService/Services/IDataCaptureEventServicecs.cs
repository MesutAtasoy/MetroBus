using Application.Common.Models;

namespace DataCaptureService.Services;

public interface IDataCaptureEventService
{
     void SendMessage(FileModel model, string sessionId, string correlationId);
}
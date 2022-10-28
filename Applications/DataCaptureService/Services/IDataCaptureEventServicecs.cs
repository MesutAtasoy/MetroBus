using Application.Common.Models;

namespace DataCaptureService.Services;

public interface IDataCaptureEventService
{
     Task SendMessageAsync(FileModel model, string sessionId, string correlationId);
}
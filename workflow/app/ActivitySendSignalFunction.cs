using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace WorkflowApp
{
    public class ActivitySendSignalFunction
    {
        [FunctionName("SendNeedsApprovalSignal")]
        public void SendSignal(
            [ActivityTrigger] string blobName,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            log.LogInformation($"Executing Activity: SendNeedsApprovalSignal - {blobName}");
            /*signalMessages.Add(new SignalRMessage
            {
                Target = "signalSend",
                Arguments = new[]
                {
                    new {
                        Name = signalInfo.SignalName,
                        Level = signalInfo.SignalType.ToString(),
                        Data = signalInfo.Metadata
                    }
                }
            });*/
        }
    }
}
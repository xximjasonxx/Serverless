using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace WorkflowApp
{
    public class ActivitySendSignalFunction
    {
        [FunctionName("SendNeedsApprovalSignal")]
        public void SendNeedsApprovalSignal(
            [ActivityTrigger] IDurableActivityContext context,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            var blobName = context.GetInput<string>();
            log.LogInformation($"Executing Activity: SendNeedsApprovalSignal - {blobName}");

            signalMessages.Add(new SignalRMessage
            {
                Target = "signalSend",
                Arguments = new[]
                {
                    new {
                        Name = "Image.NeedsApproval",
                        Level = "Warning",
                        Data = new
                        {
                            lookupLocation = $"view/{blobName}"
                        }
                    }
                }
            });
        }
    }
}
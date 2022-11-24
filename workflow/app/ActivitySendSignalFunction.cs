using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace WorkflowApp
{
    public class ActivitySendSignalFunction
    {
        [FunctionName("SendSignalString")]
        public void SendSignal(
            [ActivityTrigger] string message,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            log.LogInformation($"Executing Activity: SendSignal - {message}");
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

        [FunctionName("SendSignal")]
        public void SendSignalTyped(
            [ActivityTrigger] SignalInfo message,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            log.LogInformation($"Executing Activity: SendSignalTyped - {message.SignalName}");
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
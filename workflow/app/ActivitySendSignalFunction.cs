using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

namespace WorkflowApp
{
    public class ActivitySendSignalFunction
    {
        [FunctionName("SendSignal")]
        public void SendSignal(
            [ActivityTrigger] SignalInfo signalInfo,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            log.LogInformation($"Executing Activity: SendSignal - {JsonConvert.SerializeObject(signalInfo)}");
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
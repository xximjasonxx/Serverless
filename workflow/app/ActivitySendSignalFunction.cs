
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

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
            signalMessages.Add(new SignalRMessage
            {
                Target = "MessageSend",
                Arguments = new[]
                {
                    new {
                        Name = signalInfo.SignalName,
                        Level = signalInfo.SignalType.ToString(),
                        Data = signalInfo.Metadata
                    }
                }
            });
        }
    }
}
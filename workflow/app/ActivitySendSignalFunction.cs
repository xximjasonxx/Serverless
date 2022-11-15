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
        public async Task Run(
            [ActivityTrigger] SignalInfo signalInfo,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRConnection")]IAsyncCollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            await signalMessages.AddAsync(new SignalRMessage
            {
                
            });
        }
    }
}

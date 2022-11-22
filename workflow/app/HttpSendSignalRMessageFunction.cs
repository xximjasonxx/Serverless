using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace WorkflowApp
{
    public class HttpSendSignalRMessageFunction
    {
        [FunctionName("SendSignalTest")]
        public IActionResult SendSignalTest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "signal/send")] HttpRequest req,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            signalMessages.Add(new SignalRMessage
            {
                Target = "signalSend",
                Arguments = new[] { "Hello from Azure Functions!" }
            });

            return new CreatedResult(string.Empty, "Created");
        }
    }
}

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
using System.Collections.Generic;

namespace WorkflowApp
{
    public class HttpSendSignalRMessageFunction
    {
        [FunctionName("SendSignalTest")]
        public IActionResult SendSignalTest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "signal/send/{level}")] HttpRequest req,
            string level,
            [SignalR(HubName = "Signals", ConnectionStringSetting = "SignalRServiceConnectionString")]ICollector<SignalRMessage> signalMessages,
            ILogger log)
        {
            signalMessages.Add(new SignalRMessage
            {
                Target = "signalSend",
                Arguments = new[]
                {
                    new {
                        Name = "Image.Processed",
                        Level = level,
                        BlobName = "myblob",
                        Data = new Dictionary<string, string>
                        {
                            { "lookupLocation", "/results/myblob.jpg" }
                        }
                    }
                }
            });

            return new CreatedResult(string.Empty, "Created");
        }
    }
}

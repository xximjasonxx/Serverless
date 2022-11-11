using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WorkflowApp
{
    public class ActivityGetAcceptabilityScoreFunction
    {
        [FunctionName("GetAcceptablityScore")]
        public async Task<int> Run(
            [ActivityTrigger] IDurableActivityContext context,
            [Blob("received/{blobName}", FileAccess.Read)] Stream receivedStream,
            [CognitiveSericesBinding] CogniveServicesClient cognitiveServicesClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return 0;
        }
    }
}

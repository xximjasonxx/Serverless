using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WorkflowApp
{
    public static class OrchestrateImageProcessingFunction
    {
        [FunctionName("OrchestrateProcessImage")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)    
        {
            var blobName = context.GetInput<string>();

            // start an activity to determine acceptable score
            var acceptableScore = await context.CallActivityAsync<int>("GetAcceptablityScore", new { blobName });
        }
    }
}
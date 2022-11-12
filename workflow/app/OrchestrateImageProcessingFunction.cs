using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace WorkflowApp
{
    public static class OrchestrateImageProcessingFunction
    {
        [FunctionName("OrchestrateProcessImage")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)    
        {
            var blobName = context.GetInput<string>();

            if (string.IsNullOrEmpty(blobName) == false)
            {
                // start an activity to determine acceptable score
                var acceptableScore = await context.CallActivityAsync<int>("GetAcceptablityScore", blobName);
            }
            
            return new List<string> { "hello", "world" };
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Binding;

namespace WorkflowApp
{
    public class ActivityGetAcceptabilityScoreFunction
    {
        [FunctionName("GetAcceptablityScore")]
        public async Task<int> Run(
            [ActivityTrigger] string blobName,
            [Blob("received/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            [CognitiveServicesBinding] CognitiveServicesClient cognitiveServicesClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var acceptableScore = await cognitiveServicesClient.GetAcceptabilityScore(receivedBlob);

            return 0;
        }
    }
}

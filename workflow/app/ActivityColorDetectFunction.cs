using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using WorkflowApp.Binding;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivityColorDetectFunction
    {
        [FunctionName("DetectColor")]
        public async Task<ColorResult> DetectColor(
            [ActivityTrigger] string blobName,
            [Blob("received/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            [CognitiveServicesBinding] CognitiveServicesClient cognitiveServicesClient,
            ILogger log)
        {
            log.LogInformation("Executing Activity: DetectColor");

            return await cognitiveServicesClient.DetectColors(receivedBlob);
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Binding;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivityDetectColorsFunction
    {
        [FunctionName("DetectColors")]
        public async Task<ColorResult> Run(
            [ActivityTrigger] string blobName,
            [Blob("received/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            [CognitiveServicesBinding] CognitiveServicesClient cognitiveServicesClient,
            ILogger log)
        {
            return await cognitiveServicesClient.DetectColors(receivedBlob);
        }
    }
}

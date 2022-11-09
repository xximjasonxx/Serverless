using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WorkflowApp.Binding;

namespace WorkflowApp
{
    public class BlobReceiveCheckImageFunction
    {
        [FunctionName("BlobReceiveCheckImageFunction")]
        public async Task CheckImage(
            [BlobTrigger("received/{blobName}", Connection = "StorageAccountConnection")]Stream receivedBlob,
            [CognitiveServicesBinding]CognitiveServicesClient computerVisionClient,
            string blobName,
            ILogger log)
        {
            if (computerVisionClient == null)
            {
                log.LogError("ComputerVisionClient is null");
            }

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {receivedBlob.Length} Bytes");
        }
    }
}

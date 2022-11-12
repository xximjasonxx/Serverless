using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace WorkflowApp
{
    public class BlobImageReceivedFunction
    {
        [FunctionName("BlobReceiveImageFunction")]
        public async Task ImageReceived(
            [BlobTrigger("received/{blobName}", Connection = "StorageAccountConnection")]Stream receivedBlob,
            [DurableClient] IDurableClient starter,
            string blobName,
            ILogger log)
        {
            var instance = await starter.GetStatusAsync(blobName);
            if (instance == null)
            {
                log.LogInformation($"Starting orchestration for {blobName}");
                await starter.StartNewAsync("OrchestrateProcessImage", blobName, blobName);
            }
            else
            {
                log.LogInformation($"Orchestration for {blobName} already exists");
                await starter.PurgeInstanceHistoryAsync(blobName);
            }
        }
    }
}

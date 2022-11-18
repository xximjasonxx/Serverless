
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.IO;

namespace WorkflowApp
{
    public class ActivitySaveImageFunction
    {
        [FunctionName("SaveImage")]
        public async Task SaveImage(
            [ActivityTrigger] string blobName,
            [Blob("received/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            [Blob("original/{blobName}", FileAccess.Write, Connection = "StorageAccountConnection")] Stream originalBlob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await receivedBlob.CopyToAsync(originalBlob);
        }
    }
}
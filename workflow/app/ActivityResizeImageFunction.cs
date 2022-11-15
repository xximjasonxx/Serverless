using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Binding;

namespace WorkflowApp
{
    public class ActivityResizeImageFunction
    {
        [FunctionName("ResizeImage")]
        public async Task<string> Run(
            [ActivityTrigger] string blobName,
            [Blob("resized/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            ILogger log)
        {
            return string.Empty;
        }
    }
}

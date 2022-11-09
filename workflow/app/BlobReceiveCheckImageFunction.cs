using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace app
{
    public class BlobReceiveCheckImageFunction
    {
        [FunctionName("BlobReceiveCheckImageFunction")]
        public async Task CheckImage(
            [BlobTrigger("received/{blobName}", Connection = "StorageAccountConnection")]Stream receivedBlob,
            string blobName,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {receivedBlob.Length} Bytes");
        }
    }
}

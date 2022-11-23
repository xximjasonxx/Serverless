using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace WorkflowApp
{
    public class HttpReceiveImageFunction
    {
        [FunctionName("ReceiveImage")]
        public async Task<IActionResult> ReceiveImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "image")] HttpRequest req,
            [Blob("received", FileAccess.Write, Connection = "StorageAccountConnection")] BlobContainerClient containerClient,
            [DurableClient] IDurableClient starter,
            ILogger log)
        {
            log.LogInformation("Executing Http Call: ReceiveImage");
            var formdata = await req.ReadFormAsync();
            var file = formdata.Files.FirstOrDefault(x => x.Name == "image");
            if (file == null)
            {
                return new BadRequestObjectResult("No key 'image' file found in form data");
            }

            var blobName = $"{Guid.NewGuid().ToString()}-{file.FileName}";
            using var fileStream = file.OpenReadStream();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream);

            var instance = await starter.GetStatusAsync(blobName);
            if (instance == null)
            {
                log.LogInformation($"Starting orchestration for {blobName}");
                await starter.StartNewAsync("OrchestrateProcessImage", blobName, blobName);
                return starter.CreateCheckStatusResponse(req, blobName);
            }

            return new ConflictResult();
        }
    }
}

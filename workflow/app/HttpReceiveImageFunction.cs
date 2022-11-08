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

namespace WorkflowApp
{
    public class HttpReceiveImageFunction
    {
        [FunctionName("HttpReceiveImageFunction")]
        public async Task<IActionResult> ReceiveImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "image")] HttpRequest req,
            [Blob("received", FileAccess.Write, Connection = "StorageAccountConnection")] BlobContainerClient containerClient,
            ILogger log)
        {
            var formdata = await req.ReadFormAsync();
            var file = formdata.Files.FirstOrDefault(x => x.Name == "image");
            if (file == null)
            {
                return new BadRequestObjectResult("No key 'image' file found in form data");
            }

            var filenamePrefix = Guid.NewGuid().ToString();
            var newfileName = $"{filenamePrefix}-{file.FileName}";
            using var fileStream = file.OpenReadStream();
            var blobClient = containerClient.GetBlobClient(newfileName);
            await blobClient.UploadAsync(fileStream);

            return new AcceptedResult($"/status/{filenamePrefix}", $"Created blob {newfileName}");
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WorkflowApp
{
    public class HttpReceiveImageFunction
    {
        [FunctionName("HttpReceiveImageFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "image")] HttpRequest req,
            [Blob("raw", FileAccess.Write, Connection = "StorageAccountConnection")] BlobContainerClient containerClient,
            ILogger log)
        {
            var imageStream = req.Body;
            if (imageStream == null)
            {
                return new BadRequestObjectResult("No image provided");
            }

            if (imageStream?.Length == 0)
            {
                return new BadRequestObjectResult("Empty image provided");
            }

            var blobName = Guid.NewGuid().ToString();
            var blobClient = containerClient.GetBlobClient(blobName);
            
            var contentInfo = await blobClient.UploadAsync(imageStream);
            var rawResponse = contentInfo.GetRawResponse();

            return new CreatedResult($"/image/{blobName}", $"response: {rawResponse.ReasonPhrase}");
        }
    }
}

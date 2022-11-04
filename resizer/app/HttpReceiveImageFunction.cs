using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace ImageApi
{
    public class HttpReceiveImageFunction
    {
        [FunctionName("HttpReceiveImageFunction")]
        public async Task<IActionResult> ReceiveImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "image")] HttpRequest req,
            //[Blob("raw", Connection = "StoageAccountConnection")] BlobContainerClient containerClient,
            ILogger log)
        {
            var imageStream = req.Body;
            if (imageStream == null)
            {
                return new BadRequestObjectResult("No image provided");
            }

            /*if (imageStream.Length == 0)
            {
                return new BadRequestObjectResult("Empty image provided");
            }*/

            var blobName = Guid.NewGuid().ToString();
            /*var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(imageStream);*/

            return new CreatedResult($"/image/{blobName}", blobName);
        }
    }
}

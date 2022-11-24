using System;
using System.IO;
using System.Linq;
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
            [Blob("raw", FileAccess.Write, Connection = "StorageAccountConnection")] BlobContainerClient containerClient,
            ILogger log)
        {
            var formdata = await req.ReadFormAsync();
            var file = formdata.Files.FirstOrDefault(x => x.Name == "image");
            if (file == null)
            {
                return new BadRequestObjectResult("No key 'image' file found in form data");
            }

            if (file.Length == 0)
            {
                return new BadRequestObjectResult("Empty image provided");
            }

            var blobName = Guid.NewGuid().ToString();
            var blobClient = containerClient.GetBlobClient(blobName);
            var contentInfo = await blobClient.UploadAsync(file.OpenReadStream());
            var rawResponse = contentInfo.GetRawResponse();

            return new CreatedResult($"/image/{blobName}", $"response: {rawResponse.ReasonPhrase}");
        }
    }
}

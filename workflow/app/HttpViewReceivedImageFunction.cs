using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using Azure.Storage.Blobs;

namespace WorkflowApp
{
    public class HttpViewReceivedImageFunction
    {
        [FunctionName("ViewReceivedImage")]
        public async Task<IActionResult> ViewReceivedImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "view/{blobName}")] HttpRequest req,
            [Blob("received/{blobName}", FileAccess.Read)] BlobClient receivedblob,
            ILogger log)
        {
            var blobExists = await receivedblob.ExistsAsync();

            if (blobExists == false)
                return new NotFoundResult();

            IImageFormat format;
            var memStream = new MemoryStream();
            await receivedblob.DownloadToAsync(memStream);
            memStream.Position = 0;

            using (var image = Image.Load(memStream, out format))
            {
                memStream.Position = 0;
                return new FileStreamResult(memStream, format.DefaultMimeType);
            }
        }
    }
}

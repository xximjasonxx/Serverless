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

namespace WorkflowApp
{
    public class HttpViewReceivedImageFunction
    {
        [FunctionName("ViewReceivedImage")]
        public IActionResult ViewReceivedImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "view/{blobName}")] HttpRequest req,
            [Blob("received/{blobName}", FileAccess.Read)] Stream receivedblob,
            ILogger log)
        {
            if (receivedblob.Length == 0)
                return new NotFoundResult();

            IImageFormat format;
            using (var image = Image.Load(receivedblob, out format))
            {
                return new FileStreamResult(receivedblob, format.DefaultMimeType);
            }
        }
    }
}

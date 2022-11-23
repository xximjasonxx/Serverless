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
    public class HttpGetImageResultFunction
    {
        [FunctionName("GetImageResult")]
        public IActionResult GetImageResult(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "results/{blobName}")] HttpRequest req,
            [CosmosDB(
                databaseName: "images",
                collectionName: "image_data",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{blobName}",
                PartitionKey = "{blobName}")] dynamic result,
            ILogger log)
        {
            if (result == null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}

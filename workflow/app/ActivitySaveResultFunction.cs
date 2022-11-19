
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivitySaveResultFunction
    {
        [FunctionName("SaveResult")]
        public void SaveResult(
            [ActivityTrigger] SaveResult incomingResult,
            [CosmosDB(
                databaseName: "images",
                collectionName:"image_data",
                ConnectionStringSetting = "CosmosDBConnection")]out SaveResult outgoingResult,
            ILogger log)
        {
            log.LogInformation($"Saving result for image {incomingResult.BlobName}");
            outgoingResult = incomingResult;
        }
    }
}

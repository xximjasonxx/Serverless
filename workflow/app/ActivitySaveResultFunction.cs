using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivitySaveResultFunction
    {
        [FunctionName("SaveResult")]
        public async Task SaveResult(
            [ActivityTrigger] IDurableActivityContext context,
            [CosmosDB("images", "image_data", ConnectionStringSetting = "CosmosDBConnection")] ICollector<SaveResult> saveResults,
            ILogger log)
        {
            var saveResult = context.GetInput<SaveResult>();

            log.LogInformation($"Saving result for image {saveResult.BlobName}");
            saveResults.Add(saveResult);
        }
    }
}

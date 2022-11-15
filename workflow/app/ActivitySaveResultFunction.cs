
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivitySaveResultFunction
    {
        [FunctionName("SaveResults")]
        public async Task Run(
            [ActivityTrigger] SaveResult saveResult,
            [CosmosDB(
                databaseName: "Results",
                collectionName: "ImageResults",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<SaveResult> results,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
        }
    }
}

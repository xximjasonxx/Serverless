using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Binding;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public class ActivityDetectBrandsFunction
    {
        [FunctionName("DetectBrands")]
        public async Task<BrandResults> Run(
            [ActivityTrigger] string blobName,
            [Blob("received/{blobName}", FileAccess.Read, Connection = "StorageAccountConnection")] Stream receivedBlob,
            [CognitiveServicesBinding] CognitiveServicesClient cognitiveServicesClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            return await cognitiveServicesClient.DetectBrands(receivedBlob);
        }
    }
}

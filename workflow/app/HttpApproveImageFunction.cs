using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace WorkflowApp
{
    public class HttpApproveImageFunction
    {
        [FunctionName("ApproveImage")]
        public async Task<IActionResult> ApproveImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "approve/{instanceId}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string instanceId,
            ILogger log)
        {
            log.LogInformation("Executing Http Call: ApproveImage");
            var statusResult = await client.GetStatusAsync(instanceId);
            if (statusResult == null)
                return new BadRequestObjectResult($"Instance {instanceId} could not be approved");

            await client.RaiseEventAsync(statusResult.InstanceId, "Image.Approved");
            return new AcceptedResult();
        }
    }
}

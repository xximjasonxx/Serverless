using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using WorkflowApp.Models;

namespace WorkflowApp
{
    public static class OrchestrateImageProcessingFunction
    {
        [FunctionName("OrchestrateProcessImage")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)    
        {
            var blobName = context.GetInput<string>();

            // start an activity to determine acceptable score
            var faceCount = await context.CallActivityAsync<int>("GetAcceptablityScore", blobName);

            // if acceptable score is greater than zero, request approval
            if (faceCount > 0)
            {
                // send approval request message
                /*await context.CallActivityAsync("SendSignal", new SignalInfo
                {
                    SignalType = SignalType.Warning,
                    SignalName = SignalInfo.ApprovalRequestEvent,
                    Metadata = new Dictionary<string, string>
                    {
                        { "blobLocation", $"image/raw/{blobName}" }
                    }
                });*/

                // wait for the approval
                var approvalResponse = context.WaitForExternalEvent<bool>("Image.Approved");
                await Task.WhenAny(new List<Task> { approvalResponse });
            }

            // save the result
            var saveImageTask = context.CallActivityAsync("SaveImage", blobName);
            var colorDetectTask = context.CallActivityAsync<ColorResult>("DetectColor", blobName);
            await Task.WhenAll(new List<Task>
            {
                saveImageTask,
                colorDetectTask
            });

            var saveResult = new SaveResult
            {
                BlobName = blobName,
                ColorResult = colorDetectTask.Result
            };

            // save everything
            await context.CallActivityAsync("SaveResult", saveResult);

            // send notification of save
            /*await context.CallActivityAsync("SendSignal", new SignalInfo
            {
                SignalType = SignalType.Success,
                SignalName = "Image.Processed",
                Metadata = new Dictionary<string, string>
                {
                    { "lookupLocation", $"results/{blobName}" }
                }
            });*/
        }
    }
}
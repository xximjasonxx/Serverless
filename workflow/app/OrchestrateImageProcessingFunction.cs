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
            var acceptableScore = await context.CallActivityAsync<int>("GetAcceptablityScore", blobName);

            // if acceptable score is greater than zero, request approval
            if (acceptableScore > 0)
            {
                // send approval request message
                await context.CallActivityAsync("SendSignal", new SignalInfo
                {
                    SignalType = SignalType.Warning,
                    SignalName = SignalInfo.ApprovalRequestEvent,
                    Metadata = new Dictionary<string, string>
                    {
                        { "blobLocation", $"image/raw/{blobName}" }
                    }
                });

                // wait for the approval
                var approvalResponse = context.WaitForExternalEvent<bool>("Image.Approved");
                await Task.WhenAny(new List<Task> { approvalResponse });
            }

            // now fan out to perform various analysis of the image
            // the callers will retunr when they are done
            var brandDetectionResults = context.CallActivityAsync<BrandResults>("DetectBrands", blobName);
            var adultContextDetectResults = context.CallActivityAsync<CategoryResults>("DetectCategories", blobName);
            var colorDetectResults = context.CallActivityAsync<ColorResult>("DetectColors", blobName);
            var resizeAction = context.CallActivityAsync<string>("ResizeImage", blobName);

            await Task.WhenAll(new List<Task> { brandDetectionResults, adultContextDetectResults, colorDetectResults, resizeAction });
            var results = SaveResult.Create(
                brandDetectionResults.Result,
                adultContextDetectResults.Result,
                colorDetectResults.Result,
                resizeAction.Result,
                blobName);

            await context.CallActivityAsync("SaveResults", results);

            // send notification of save
            await context.CallActivityAsync("SendSignal", new SignalInfo
            {
                SignalType = SignalType.Information,
                SignalName = "Image.Processed",
                Metadata = new Dictionary<string, string>
                {
                    { "lookupLocation", $"results/{blobName}" }
                }
            });
        }
    }
}
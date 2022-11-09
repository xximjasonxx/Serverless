using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using WorkflowApp;
using WorkflowApp.Binding;

[assembly: WebJobsStartup(typeof(WorkflowAppStartup))]
namespace WorkflowApp
{
    public class WorkflowAppStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<CognitiveServicesBinding>();
        }
    }
}
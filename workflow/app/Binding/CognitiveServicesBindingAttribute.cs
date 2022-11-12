
using System;
using Microsoft.Azure.WebJobs.Description;

namespace WorkflowApp.Binding;

[Binding]
[AttributeUsage(AttributeTargets.Parameter)]
public class CognitiveServicesBindingAttribute : Attribute
{
    [AppSetting(Default = "CognitiveServicesEndpoint")]
    public string Endpoint { get; set; }

    [AppSetting(Default = "CognitiveServicesKey")]
    public string Key { get; set; }

    [AppSetting(Default = "CognitiveServicesLocation")]
    public string Location { get; set; }

}
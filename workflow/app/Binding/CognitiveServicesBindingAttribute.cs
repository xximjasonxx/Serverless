
using System;
using Microsoft.Azure.WebJobs.Description;

namespace WorkflowApp.Binding;

[Binding]
[AttributeUsage(AttributeTargets.Parameter)]
public class CognitiveServicesBindingAttribute : Attribute
{
    [AutoResolve]
    public string Endpoint { get; set; }

}
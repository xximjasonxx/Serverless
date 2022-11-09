using Microsoft.Azure.WebJobs.Host.Config;

namespace WorkflowApp.Binding
{
    public class CognitiveServicesBinding : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<CognitiveServicesBindingAttribute>();
            rule.BindToInput<CognitiveServicesClient>(BuildClientFromAttribute);
        }

        private CognitiveServicesClient BuildClientFromAttribute(CognitiveServicesBindingAttribute arg)
        {
            return new CognitiveServicesClient();
        }
    }
}
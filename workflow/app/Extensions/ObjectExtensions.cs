using Newtonsoft.Json;

namespace WorkflowApp.Extensions
{
    public static class ObjectExtensions
    {
        public static string AsString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
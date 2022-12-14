using System.Collections.Generic;

namespace WorkflowApp
{
    public class SignalInfo
    {
        public SignalType SignalType { get; internal set; }
        public string SignalName { get; internal set; }
        public Dictionary<string, string> Metadata { get; internal set; }
        public string BlobName { get; internal set; }
    }

    public enum SignalType
    {
        Warning,
        Error,
        Success
    }
}
using System.Collections.Generic;

namespace WorkflowApp.Models
{
    public class BrandResult
    {
        public double ConfidenceScore { get; set; }
        public string BrandName { get; set; }
    }

    public class BrandResults
    {
        public List<BrandResult> Results { get; set; }
    }
}
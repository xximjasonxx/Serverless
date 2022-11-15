using System.Collections.Generic;

namespace WorkflowApp.Models
{
    public class CategoryResult
    {
        public string CategoryName { get; set; }
        public double Score { get; set; }
    }

    public class CategoryResults
    {
        public List<CategoryResult> Results { get; set; }
    }
}
using System.Collections.Generic;

namespace WorkflowApp.Models
{
    public class ColorResult
    {
        public string AccentColor { get; set; }
        public string DominantColorBackground { get; set; }
        public string DominantColorForeground { get; set; }
        public bool IsBlackAndWhite { get; set; }

        public List<string> DominentColors { get; set; }
    }
}
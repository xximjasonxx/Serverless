namespace WorkflowApp.Models
{
    public class SaveResult
    {
        private SaveResult() { }

        public string BlobName { get; private set; }
        public string ResizePath { get; private set; }
        public BrandResults BrandResults { get; private set; }
        public CategoryResults CategoryResults { get; private set; }
        public ColorResult ColorResult { get; private set; }

        public static SaveResult Create(BrandResults brandResults, CategoryResults categoryResults,
            ColorResult colorResult, string resizePath, string blobName)
        {
            return new SaveResult()
            {
                BlobName = blobName,
                ResizePath = resizePath,
                BrandResults = brandResults,
                CategoryResults = categoryResults,
                ColorResult = colorResult
            };
        }
    }
}
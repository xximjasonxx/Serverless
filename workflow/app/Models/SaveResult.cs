namespace WorkflowApp.Models
{
    public class SaveResult
    {
        public string BlobName { get; init; }
        public string BlobPath { get => $"/image/{BlobName}"; }
        public ColorResult ColorResult { get; init; }
    }
}
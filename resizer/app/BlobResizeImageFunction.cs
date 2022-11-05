using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace ImageApi
{
    public class BlobResizeImageFunction
    {
        [FunctionName("BlobResizeImageFunction")]
        public void Run(
            [BlobTrigger("raw/{blobName}", Connection = "StorageAccountConnection")]Stream rawBlob,
            string blobName,
            [Blob("resized/{blobName}", FileAccess.Write, Connection = "StorageAccountConnection")]Stream resizeBlob,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processing blob\n Name:{blobName} \n Size: {rawBlob.Length} Bytes");

            IImageFormat format;
            using Image image = Image.Load(rawBlob, out format);
            int width = image.Width / 2;
            int height = image.Height / 2;

            image.Mutate(x => x.Resize(width, height));
            image.SaveAsync(resizeBlob, format);

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {resizeBlob.Length} Bytes");
        }
    }
}

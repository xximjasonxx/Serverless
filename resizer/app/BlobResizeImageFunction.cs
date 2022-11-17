using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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
        public async Task ResizeImage(
            [BlobTrigger("raw/{blobName}", Connection = "StorageAccountConnection")]Stream rawBlob,
            string blobName,
            [Blob("resized/{blobName}", FileAccess.Write, Connection = "StorageAccountConnection")]BlobClient resizeBlob,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processing blob\n Name:{blobName} \n Size: {rawBlob.Length} Bytes");

            using (var memStream = new MemoryStream())
            {
                IImageFormat format;
                using (var image = Image.Load(rawBlob, out format))
                {
                    var newWidth = (int)Math.Round(image.Width * .25m);
                    var newHeight = (int)Math.Round(image.Height * .25m);

                    image.Mutate(c => c.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));

                    await image.SaveAsync(memStream, format);
                    memStream.Position = 0;

                    await resizeBlob.UploadAsync(memStream);
                }
            }
        }
    }
}

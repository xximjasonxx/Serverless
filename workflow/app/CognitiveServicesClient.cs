
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WorkflowApp
{
    public class CognitiveServicesClient
    {
        private ComputerVisionClient _computerVisionClient;

        public CognitiveServicesClient(string key, string endpoint, string location)
        {
            _computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };
        }

        public async Task<int> GetAcceptabilityScore(Stream blobSream)
        {
            var featureTypes = new List<VisualFeatureTypes?> { VisualFeatureTypes.Color };
            var analysisResult = await _computerVisionClient.AnalyzeImageInStreamAsync(blobSream, featureTypes);

            var score = 0;
            if (analysisResult.Color.DominantColorBackground == "Pink")
                score += 1;

            if (analysisResult.Color.DominantColorForeground == "Pink")
                score += 1;

            return score;
        }
    }
}
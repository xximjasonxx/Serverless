
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using WorkflowApp.Models;

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

        public async Task<int> GetFaceCount(Stream blobSream)
        {
            var featureTypes = new List<VisualFeatureTypes?> { VisualFeatureTypes.Faces };
            var analysisResult = await _computerVisionClient.AnalyzeImageInStreamAsync(blobSream, featureTypes);

            return analysisResult.Faces.Count;
        }

        public async Task<ColorResult> DetectColors(Stream receivedBlob)
        {
            var featureTypes = new List<VisualFeatureTypes?> { VisualFeatureTypes.Color };
            var analysisResult = await _computerVisionClient.AnalyzeImageInStreamAsync(receivedBlob, featureTypes);

            return new ColorResult
            {
                AccentColor = analysisResult.Color.AccentColor,
                DominantColorBackground = analysisResult.Color.DominantColorBackground,
                DominantColorForeground = analysisResult.Color.DominantColorForeground,
                DominentColors = analysisResult.Color.DominantColors.ToList()
            };
        }

        public async Task<CategoryResults> DetectCategories(Stream receivedBlob)
        {
            var featureTypes = new List<VisualFeatureTypes?> { VisualFeatureTypes.Categories };
            var analysisResult = await _computerVisionClient.AnalyzeImageInStreamAsync(receivedBlob, featureTypes);

            return new CategoryResults
            {
                Results = analysisResult.Categories.Select(c => new CategoryResult
                {
                    CategoryName = c.Name,
                    Score = c.Score
                }).ToList()
            };
        }

        public async Task<BrandResults> DetectBrands(Stream receivedBlob)
        {
            var featureTypes = new List<VisualFeatureTypes?> { VisualFeatureTypes.Brands };
            var analysisResult = await _computerVisionClient.AnalyzeImageInStreamAsync(receivedBlob, featureTypes);

            return new BrandResults
            {
                Results = analysisResult.Brands.Select(x => new BrandResult
                {
                    BrandName = x.Name,
                    ConfidenceScore = x.Confidence
                }).ToList()
            };
        }
    }
}
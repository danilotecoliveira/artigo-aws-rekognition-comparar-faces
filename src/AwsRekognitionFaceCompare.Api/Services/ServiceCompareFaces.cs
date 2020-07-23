using System.Linq;
using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class ServiceCompareFaces : IServiceCompareFaces
    {
        private readonly IServiceUtils _serviceUtils;
        private readonly AmazonRekognitionClient _rekognitionClient;

        public ServiceCompareFaces(IServiceUtils serviceUtils)
        {
            _serviceUtils = serviceUtils;
            _rekognitionClient = new AmazonRekognitionClient();
        }

        public async Task<FaceMatchResponse> CompareFacesAsync(string sourceImage, string targetImage)
        {
            var imageSource = new Amazon.Rekognition.Model.Image();
            imageSource.Bytes = _serviceUtils.ConvertImageToMemoryStream(sourceImage);

            var imageTarget = new Amazon.Rekognition.Model.Image();
            imageTarget.Bytes = _serviceUtils.ConvertImageToMemoryStream(targetImage);

            var request = new CompareFacesRequest
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = 80f
            };

            var response = await _rekognitionClient.CompareFacesAsync(request);
            var hasMatch = response.FaceMatches.Any();

            if (!hasMatch)
            {
                return new FaceMatchResponse(hasMatch);
            }

            var convertImage = _serviceUtils.ConvertImageToMemoryStream(sourceImage);
            var drawnImage = _serviceUtils.Drawing(convertImage, response.SourceImageFace);
            var imageBase64 = _serviceUtils.ConvertImageToBase64(drawnImage);
            
            var similarity = response.FaceMatches.FirstOrDefault().Similarity;

            return new FaceMatchResponse(hasMatch, similarity, imageBase64);            
        }
    }

    public interface IServiceCompareFaces 
    {
        Task<FaceMatchResponse> CompareFacesAsync(string sourceImage, string targetImage);
    }
}
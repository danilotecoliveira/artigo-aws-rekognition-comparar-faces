using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class ServiceDetectFaces : IServiceDetectFaces
    {
        private readonly AmazonRekognitionClient _rekognitionClient;

        public ServiceDetectFaces()
        {
            _rekognitionClient = new AmazonRekognitionClient();
        }

        public async Task<FindFacesResponse> DetectFacesAsync(string sourceImage)
        {
            var imageSource = new Image();
            imageSource.Bytes = ConvertImageToMemoryStream(sourceImage);

            DetectFacesRequest request = new DetectFacesRequest
            {
                Attributes = new List<string>{ "DEFAULT" },
                Image = imageSource
            }
        }
    }

    public interface IServiceDetectFaces
    {
        Task<FindFacesResponse> DetectFacesAsync(string sourceImage);
    }
}

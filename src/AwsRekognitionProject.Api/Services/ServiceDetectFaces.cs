using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using AwsRekognitionProject.Api.Domain;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Services
{
    public class ServiceDetectFaces : IServiceDetectFaces
    {
        private readonly IServiceUtils _serviceUtils;
        private readonly AmazonRekognitionClient _rekognitionClient;

        public ServiceDetectFaces(IServiceUtils serviceUtils)
        {
            _serviceUtils = serviceUtils;
            _rekognitionClient = new AmazonRekognitionClient();
        }

        public async Task<FindFacesResponse> DetectFacesAsync(string sourceImage)
        {
            var imageSource = new Image();
            imageSource.Bytes = _serviceUtils.ConvertImageToMemoryStream(sourceImage);

            var request = new DetectFacesRequest
            {
                Attributes = new List<string>{ "DEFAULT" },
                Image = imageSource
            };
            
            var response = await _rekognitionClient.DetectFacesAsync(request);

            var convertImage = _serviceUtils.ConvertImageToMemoryStream(sourceImage);
            var fileName = _serviceUtils.Drawing(convertImage, response.FaceDetails);

            return new FindFacesResponse(fileName);
        }
    }
}
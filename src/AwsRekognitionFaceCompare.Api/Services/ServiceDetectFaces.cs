using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using AwsRekognitionFaceCompare.Api.Entities;
using System;

namespace AwsRekognitionFaceCompare.Api.Services
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
            var drawnImage = _serviceUtils.Drawing(convertImage, response.FaceDetails);
            var imageBase64 = _serviceUtils.ConvertImageToBase64(drawnImage);
            
            response.FaceDetails.ForEach(f => {
                Console.WriteLine(f.BoundingBox.Left);
                Console.WriteLine(f.BoundingBox.Top);
                Console.WriteLine(f.BoundingBox.Width);
                Console.WriteLine(f.BoundingBox.Height);
            });

            return null;
        }
    }

    public interface IServiceDetectFaces
    {
        Task<FindFacesResponse> DetectFacesAsync(string sourceImage);
    }
}

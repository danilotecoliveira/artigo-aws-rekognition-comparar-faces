using Amazon.Rekognition;
using System.Threading.Tasks;
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
            return null;
        }
    }

    public interface IServiceDetectFaces
    {
        Task<FindFacesResponse> DetectFacesAsync(string sourceImage);
    }
}

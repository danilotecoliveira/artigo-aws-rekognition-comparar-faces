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
            // Converte a imagem fonte em um objeto MemoryStream
            var imageSource = new Image();
            imageSource.Bytes = _serviceUtils.ConvertImageToMemoryStream(sourceImage);

            // Configura o objeto que fará o request para o AWS Rekognition
            var request = new DetectFacesRequest
            {
                Attributes = new List<string>{ "DEFAULT" },
                Image = imageSource
            };

            // Faz a chamada do serviço de DetectFaces            
            var response = await _rekognitionClient.DetectFacesAsync(request);
            // Chama a função de desenhar quadrados e pega a URL gerada
            var fileName = _serviceUtils.Drawing(imageSource.Bytes, response.FaceDetails);

            // Retorna o objeto com a URL gerada
            return new FindFacesResponse(fileName);
        }
    }
}
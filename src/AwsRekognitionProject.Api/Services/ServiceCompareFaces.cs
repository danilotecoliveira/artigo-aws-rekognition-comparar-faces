using System.Linq;
using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using AwsRekognitionProject.Api.Domain;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Services
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
            // Converte a imagem fonte em um objeto MemoryStream
            var imageSource = new Amazon.Rekognition.Model.Image();
            imageSource.Bytes = _serviceUtils.ConvertImageToMemoryStream(sourceImage);

            // Converte a imagem alvo em um objeto MemoryStream
            var imageTarget = new Amazon.Rekognition.Model.Image();
            imageTarget.Bytes = _serviceUtils.ConvertImageToMemoryStream(targetImage);

            // Configura o objeto que fará o request para o AWS Rekognition
            // A propriedade SimilarityThreshold ajusta o nível de similaridade na comparação das imagens
            var request = new CompareFacesRequest
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = 80f
            };

            // Faz a chamada do serviço de CompareFaces
            var response = await _rekognitionClient.CompareFacesAsync(request);
            // Verifica se houve algum match nas imagens
            var hasMatch = response.FaceMatches.Any();

            // Se não houve match ele retorna um objeto não encontrado
            if (!hasMatch)
            {
                return new FaceMatchResponse(hasMatch, null, string.Empty);
            }

            // Com a imagem fonte e os parâmetros de retorno do match contornamos o rosto encontrado na imagem
            var fileName = _serviceUtils.Drawing(imageSource.Bytes, response.SourceImageFace);
            // Pega o percentual de similaridade da imagem encontrada
            var similarity = response.FaceMatches.FirstOrDefault().Similarity;

            // Retorna o objeto com as informações encontradas e com a URL para verificar a imagem
            return new FaceMatchResponse(hasMatch, similarity, fileName);
        }
    }
}
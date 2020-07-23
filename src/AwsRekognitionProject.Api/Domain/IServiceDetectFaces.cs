using System.Threading.Tasks;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Domain
{
    public interface IServiceDetectFaces
    {
        Task<FindFacesResponse> DetectFacesAsync(string sourceImage);
    }
}

using System.Threading.Tasks;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Domain
{
    public interface IServiceCompareFaces
    {
        Task<FaceMatchResponse> CompareFacesAsync(string sourceImage, string targetImage);
    }
}
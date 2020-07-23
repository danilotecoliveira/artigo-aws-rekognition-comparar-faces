namespace AwsRekognitionFaceCompare.Api.Entities
{
    public class FindFacesResponse
    {
        public FindFacesResponse(string drawnImageBase64)
        {
            DrawnImageBase64 = drawnImageBase64;
        }

        public string DrawnImageBase64 { get; private set; }
    }
}

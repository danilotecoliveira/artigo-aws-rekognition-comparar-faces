namespace AwsRekognitionFaceCompare.Api.Entities
{
    public class FaceMatchResponse
    {
        public FaceMatchResponse(bool match, float? similarity = null, string drawnImageBase64 = "")
        {
            Match = match;
            Similarity = similarity;
            DrawnImageBase64 = drawnImageBase64;
        }

        public bool Match { get; private set; }
        public float? Similarity { get; private set; }
        public string DrawnImageBase64 { get; private set; }
    }
}
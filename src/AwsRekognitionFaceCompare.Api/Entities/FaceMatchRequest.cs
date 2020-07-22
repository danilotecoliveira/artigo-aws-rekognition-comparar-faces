namespace AwsRekognitionFaceCompare.Api.Entities
{
    public class FaceMatchRequest
    {
        public string SourceImage { get; set; }
        public string TargetImage { get; set; }
    }
}

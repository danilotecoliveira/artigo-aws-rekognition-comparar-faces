namespace AwsRekognitionFaceCompare.Api.Entities
{
    public class FaceMatchRequest
    {
        // People
        public string SourceImage { get; set; }
        
        // Person alone
        public string TargetImage { get; set; }
    }
}

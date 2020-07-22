namespace AwsRekognitionFaceCompare.Api.Entities
{
    public class FaceMatchResponse
    {
        public float PositionLeft { get; set; }
        public float PositionHeight { get; set; }
        public float PositionTop { get; set; }
        public float PositionWidth { get; set; }
        public float Similarity { get; set; }
    }
}
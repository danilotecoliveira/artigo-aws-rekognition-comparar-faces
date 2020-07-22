using System.IO;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class CompareFaces : ICompareFaces
    {
        private readonly AmazonRekognitionClient _rekognitionClient;

        public CompareFaces()
        {
            _rekognitionClient = new AmazonRekognitionClient();
        }

        public IEnumerable<FaceMatchResult> GetFaceMatches(string sourceImage, string targetImage)
        {
            var similarityThreshold = 70f;
            sourceImage = "source.jpg";
            targetImage = "target.jpg";

            var imageSource = new Amazon.Rekognition.Model.Image();
            imageSource.Bytes = ConvertImageToMemoryStream(sourceImage);

            var imageTarget = new Amazon.Rekognition.Model.Image();
            imageTarget.Bytes = ConvertImageToMemoryStream(targetImage);

            var compareFacesRequest = new CompareFacesRequest
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = similarityThreshold
            };

            var compareFacesResponse = _rekognitionClient.CompareFacesAsync(compareFacesRequest).Result;
            var listFaces = new List<FaceMatchResult>();

            foreach (var match in compareFacesResponse.FaceMatches)
            {
                var face = match.Face;
                var position = face.BoundingBox;

                listFaces.Add(
                    new FaceMatchResult
                    {
                        PositionLeft = position.Left,
                        PositionHeight = position.Height,
                        PositionTop = position.Top,
                        PositionWidth = position.Width,
                        Similarity = match.Similarity
                    }
                );
            }

            return listFaces;
        }

        public MemoryStream ConvertImageToMemoryStream(string pathImage)
        {
            using (var fs = new FileStream(pathImage, FileMode.Open, FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                return new MemoryStream(data);
            }
        }
    }

    public interface ICompareFaces 
    {
        IEnumerable<FaceMatchResult> GetFaceMatches(string sourceImage, string targetImage);
    }
}
using System.IO;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class CompareFaces : ICompareFaces
    {
        public void Example()
        {
            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            var similarityThreshold = 70f;
            var sourceImage = "source.jpg";
            var targetImage = "target.jpg";

            Amazon.Rekognition.Model.Image imageSource = new Amazon.Rekognition.Model.Image();
            imageSource.Bytes = ConvertImageToMemoryStream(sourceImage);

            Amazon.Rekognition.Model.Image imageTarget = new Amazon.Rekognition.Model.Image();
            imageTarget.Bytes = ConvertImageToMemoryStream(targetImage);

            CompareFacesRequest compareFacesRequest = new CompareFacesRequest
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = similarityThreshold
            };

            CompareFacesResponse compareFacesResponse = rekognitionClient.CompareFacesAsync(compareFacesRequest).Result;
            List<FaceMatchResult> listFaces = new List<FaceMatchResult>();

            foreach (CompareFacesMatch match in compareFacesResponse.FaceMatches)
            {
                ComparedFace face = match.Face;
                BoundingBox position = face.BoundingBox;

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
        }

        public MemoryStream ConvertImageToMemoryStream(string pathImage)
        {
            using (FileStream fs = new FileStream(pathImage, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                return new MemoryStream(data);
            }
        }
    }

    public interface ICompareFaces 
    {
        //
    }
}
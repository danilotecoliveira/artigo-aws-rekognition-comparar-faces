using System;
using System.IO;
using System.Linq;
using Amazon.Rekognition;
using sd = System.Drawing;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
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

        public async Task<FaceMatchResponse> CompareFacesAsync(string sourceImage, string targetImage)
        {
            var imageSource = new Amazon.Rekognition.Model.Image();
            imageSource.Bytes = ConvertImageToMemoryStream(sourceImage);

            var imageTarget = new Amazon.Rekognition.Model.Image();
            imageTarget.Bytes = ConvertImageToMemoryStream(targetImage);

            var request = new CompareFacesRequest
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = 80f
            };

            var response = await _rekognitionClient.CompareFacesAsync(request);
            var hasMatch = response.FaceMatches.Any();

            if (!hasMatch)
            {
                return new FaceMatchResponse(hasMatch);
            }

            var convertImage = ConvertImageToMemoryStream(sourceImage);
            var drawnImage = Drawing(convertImage, response.SourceImageFace);
            var imageBase64 = ConvertImageToBase64(drawnImage);
            
            var similarity = response.FaceMatches.FirstOrDefault().Similarity;

            return new FaceMatchResponse(hasMatch, similarity, imageBase64);            
        }

        private sd.Image Drawing(MemoryStream imageSource, ComparedSourceImageFace faceMatch)
        {
            var image = sd.Image.FromStream(imageSource);
            var graphic = sd.Graphics.FromImage(image);
            var pen = new sd.Pen(sd.Brushes.Red, 3f);

            var left = faceMatch.BoundingBox.Left * image.Width;
            var top = faceMatch.BoundingBox.Top * image.Height;
            var width = faceMatch.BoundingBox.Width * image.Width;
            var height = faceMatch.BoundingBox.Height * image.Height;

            graphic.DrawRectangle(pen, left, top, width, height);    

            return image;
        }

        private string ConvertImageToBase64(sd.Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, image.RawFormat);
                var imageBytes = memory.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                
                return base64String;
            }
        }

        private MemoryStream ConvertImageToMemoryStream(string imageBase64)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            return new MemoryStream(bytes);
        }
    }

    public interface ICompareFaces 
    {
        Task<FaceMatchResponse> CompareFacesAsync(string sourceImage, string targetImage);
    }
}
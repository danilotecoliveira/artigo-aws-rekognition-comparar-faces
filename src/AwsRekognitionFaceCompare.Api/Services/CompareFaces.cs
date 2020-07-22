using System;
using System.IO;
using System.Linq;
using System.Drawing;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class CompareFaces : ICompareFaces
    {
        private readonly ILogger<CompareFaces> _logger;
        private readonly AmazonRekognitionClient _rekognitionClient;

        public CompareFaces(ILogger<CompareFaces> logger)
        {
            _logger = logger;
            _rekognitionClient = new AmazonRekognitionClient();
        }

        public IEnumerable<FaceMatchResponse> GetFaceMatches(string sourceImage, string targetImage)
        {
            var similarityThreshold = 90f;

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
            var listFaces = new List<FaceMatchResponse>();

            foreach (var match in compareFacesResponse.FaceMatches)
            {
                var face = match.Face;
                var position = compareFacesResponse.SourceImageFace.BoundingBox;

                listFaces.Add(
                    new FaceMatchResponse
                    {
                        PositionLeft = position.Left,
                        PositionHeight = position.Height,
                        PositionTop = position.Top,
                        PositionWidth = position.Width,
                        Similarity = match.Similarity
                    }
                );
            }

            Drawing(ConvertImageToMemoryStream(sourceImage), listFaces);

            return listFaces;
        }

        public void Drawing(MemoryStream imageSource, List<FaceMatchResponse> listFaces)
        {
            var image = System.Drawing.Image.FromStream(imageSource);
            Graphics graph = Graphics.FromImage(image);
            Pen pen = new Pen(Brushes.Red);
            var face = listFaces.FirstOrDefault();

            var left = face.PositionLeft * image.Width;
            var top = face.PositionTop * image.Height;
            var width = face.PositionWidth * image.Width;
            var height = face.PositionHeight * image.Height;

            graph.DrawRectangle(pen, left, top, width, height);    
            image.Save("myImage.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private MemoryStream ConvertImageToMemoryStream(string imageBase64)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            return new MemoryStream(bytes);
        }
    }

    public interface ICompareFaces 
    {
        IEnumerable<FaceMatchResponse> GetFaceMatches(string sourceImage, string targetImage);
    }
}
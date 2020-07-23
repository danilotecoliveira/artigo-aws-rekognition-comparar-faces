using System;
using System.IO;
using System.Drawing;
using aws = Amazon.Rekognition.Model;

namespace AwsRekognitionFaceCompare.Api.Services
{
    public class ServiceUtils : IServiceUtils
    {
        public string ConvertImageToBase64(Image image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, image.RawFormat);
                var imageBytes = memory.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                
                return base64String;
            }
        }

        public MemoryStream ConvertImageToMemoryStream(string imageBase64)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            return new MemoryStream(bytes);
        }

        public Image Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch)
        {
            var image = Image.FromStream(imageSource);
            var graphic = Graphics.FromImage(image);
            var pen = new Pen(Brushes.Red, 3f);

            var left = faceMatch.BoundingBox.Left * image.Width;
            var top = faceMatch.BoundingBox.Top * image.Height;
            var width = faceMatch.BoundingBox.Width * image.Width;
            var height = faceMatch.BoundingBox.Height * image.Height;

            graphic.DrawRectangle(pen, left, top, width, height);    

            return image;
        }
    }

    public interface IServiceUtils
    {
        string ConvertImageToBase64(Image image);
        MemoryStream ConvertImageToMemoryStream(string imageBase64);
        Image Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch);
    }
}

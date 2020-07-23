using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
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
            var boxes = new List<aws.BoundingBox>();
            
            boxes.Add(
                new aws.BoundingBox{
                    Left = faceMatch.BoundingBox.Left,
                    Top = faceMatch.BoundingBox.Top,
                    Width = faceMatch.BoundingBox.Width,
                    Height = faceMatch.BoundingBox.Height
                }
            );

            return Drawing(imageSource, boxes);
        }

        public Image Drawing(MemoryStream imageSource, List<aws.FaceDetail> faceDetails)
        {
            var boxes = new List<aws.BoundingBox>();
            
            faceDetails.ForEach(f => {
                boxes.Add(
                    new aws.BoundingBox{
                        Left = f.BoundingBox.Left,
                        Top = f.BoundingBox.Top,
                        Width = f.BoundingBox.Width,
                        Height = f.BoundingBox.Height
                    }
                );
            });
            
            return Drawing(imageSource, boxes);
        }

        private Image Drawing(MemoryStream imageSource, List<aws.BoundingBox> boxes)
        {
            var image = Image.FromStream(imageSource);
            var graphic = Graphics.FromImage(image);
            var pen = new Pen(Brushes.Red, 5f);

            boxes.ForEach(b => {
                graphic.DrawRectangle(
                    pen, 
                    b.Left * image.Width, 
                    b.Top * image.Height, 
                    b.Width * image.Width, 
                    b.Height * image.Height
                );
            });

            return image;
        }
    }

    public interface IServiceUtils
    {
        string ConvertImageToBase64(Image image);
        MemoryStream ConvertImageToMemoryStream(string imageBase64);
        Image Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch);
        Image Drawing(MemoryStream imageSource, List<aws.FaceDetail> faceDetails);
    }
}

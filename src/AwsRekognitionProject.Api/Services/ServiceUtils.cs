using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using aws = Amazon.Rekognition.Model;
using AwsRekognitionProject.Api.Domain;

namespace AwsRekognitionProject.Api.Services
{
    public class ServiceUtils : IServiceUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        // Converte uma imagem string base64 em MemoryStream
        public MemoryStream ConvertImageToMemoryStream(string imageBase64)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            return new MemoryStream(bytes);
        }

        // Função sobrecarga que recebe o parâmetro da CompareFaces
        public string Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch)
        {
            // Cria uma lista de quadrados que será desenhada na imagem
            var squares = new List<aws.BoundingBox>();
            
            // Adiciona as posições dos quadrados que serão desenhados
            squares.Add(
                new aws.BoundingBox{
                    Left = faceMatch.BoundingBox.Left,
                    Top = faceMatch.BoundingBox.Top,
                    Width = faceMatch.BoundingBox.Width,
                    Height = faceMatch.BoundingBox.Height
                }
            );

            return Drawing(imageSource, squares);
        }

        // Função sobrecarga que recebe o parâmetro da DetectFaces
        public string Drawing(MemoryStream imageSource, List<aws.FaceDetail> faceDetails)
        {
            // Cria uma lista de quadrados que será desenhada na imagem
            var squares = new List<aws.BoundingBox>();
            
            // Adiciona as posições dos quadrados que serão desenhados
            faceDetails.ForEach(f => {
                squares.Add(
                    new aws.BoundingBox{
                        Left = f.BoundingBox.Left,
                        Top = f.BoundingBox.Top,
                        Width = f.BoundingBox.Width,
                        Height = f.BoundingBox.Height
                    }
                );
            });
            
            return Drawing(imageSource, squares);
        }

        // Função que desenha os quadrados na imagem fonte
        private string Drawing(MemoryStream imageSource, List<aws.BoundingBox> squares)
        {
            //Converte o MemoryStream da imagem fonte em Imagem (System.Drawing)
            var image = Image.FromStream(imageSource);
            // O Graphics permite desenhar novos recursos na imagem fonte
            var graphic = Graphics.FromImage(image);
            // Objeto caneta que será usada para desenhar os quadrados
            var pen = new Pen(Brushes.Red, 5f);

            // Desenha os quadrados na imagem fonte
            squares.ForEach(b => {
                graphic.DrawRectangle(
                    pen, 
                    b.Left * image.Width, 
                    b.Top * image.Height, 
                    b.Width * image.Width, 
                    b.Height * image.Height
                );
            });

            // Cria o nome do arquivo com o Guid
            var fileName = $"{Guid.NewGuid()}.jpg";

            // Salva a nova imagem desenhada
            image.Save($"Images/{fileName}", ImageFormat.Jpeg);

            // Chama a função e retorna a URL com a imagem gerada
            return GetUrlImage(fileName);
        }

        // Gera uma URL com a imagem criada
        private string GetUrlImage(string fileName)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var urlImage = $"{request.Scheme}://{request.Host.ToUriComponent()}/images/{fileName}";
            
            return urlImage;
        }
    }
}

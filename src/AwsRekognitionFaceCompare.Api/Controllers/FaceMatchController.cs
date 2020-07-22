using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AwsRekognitionFaceCompare.Api.Services;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaceMatchController : ControllerBase
    {
        private readonly ICompareFaces _compareFaces;

        public FaceMatchController(ICompareFaces compareFaces)
        {
            _compareFaces = compareFaces;
        }

        [HttpGet]
        public IActionResult GetFaceMatches([FromBody] FaceMatchRequest faceMatchRequest)
        {
            try
            {
                var result = _compareFaces.GetFaceMatches(
                    faceMatchRequest.SourceImage, 
                    faceMatchRequest.TargetImage
                );

                return StatusCode(HttpStatusCode.OK.GetHashCode(), result);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}

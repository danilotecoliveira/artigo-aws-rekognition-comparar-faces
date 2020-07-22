using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AwsRekognitionFaceCompare.Api.Services;

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
        public IActionResult GetFaceMatches(string sourceImage, string targetImage)
        {
            try
            {
                var result = _compareFaces.GetFaceMatches(sourceImage, targetImage);

                return StatusCode(HttpStatusCode.OK.GetHashCode(), result);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}

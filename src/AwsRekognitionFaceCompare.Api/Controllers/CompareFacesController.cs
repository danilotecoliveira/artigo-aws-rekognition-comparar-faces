using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwsRekognitionFaceCompare.Api.Services;
using AwsRekognitionFaceCompare.Api.Entities;

namespace AwsRekognitionFaceCompare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompareFacesController : ControllerBase
    {
        private readonly IServiceCompareFaces _serviceCompareFaces;

        public CompareFacesController(IServiceCompareFaces serviceCompareFaces)
        {
            _serviceCompareFaces = serviceCompareFaces;
        }

        [HttpGet]
        public async Task<IActionResult> GetFaceMatches([FromBody] FaceMatchRequest faceMatchRequest)
        {
            try
            {
                var result = await _serviceCompareFaces.CompareFacesAsync(
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

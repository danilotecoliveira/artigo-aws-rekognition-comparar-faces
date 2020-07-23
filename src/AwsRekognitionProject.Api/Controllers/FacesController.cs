using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwsRekognitionProject.Api.Domain;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacesController : ControllerBase
    {
        private readonly IServiceDetectFaces _serviceDetectFaces;
        private readonly IServiceCompareFaces _serviceCompareFaces;

        public FacesController(
            IServiceDetectFaces serviceDetectFaces,
            IServiceCompareFaces serviceCompareFaces)
        {
            _serviceDetectFaces = serviceDetectFaces;
            _serviceCompareFaces = serviceCompareFaces;
        }

        [HttpGet("facematch")]
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

        [HttpGet("findfaces")]
        public async Task<IActionResult> GetFaceMatches([FromBody] FindFacesRequest request)
        {
            try
            {
                var response = await _serviceDetectFaces.DetectFacesAsync(
                    request.SourceImage
                );

                return StatusCode(HttpStatusCode.OK.GetHashCode(), response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}

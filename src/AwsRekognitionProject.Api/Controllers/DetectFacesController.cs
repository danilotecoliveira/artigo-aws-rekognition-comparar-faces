using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwsRekognitionProject.Api.Services;
using AwsRekognitionProject.Api.Entities;

namespace AwsRekognitionProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetectFacesController : ControllerBase
    {
        private readonly IServiceDetectFaces _serviceDetectFaces;

        public DetectFacesController(IServiceDetectFaces serviceDetectFaces)
        {
            _serviceDetectFaces = serviceDetectFaces;
        }

        [HttpGet]
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

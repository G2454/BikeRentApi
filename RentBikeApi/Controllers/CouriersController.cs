using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Couriers;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Mapping;

namespace RentBikeApi.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class CouriersController(ICourierService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourierRequest request)
        {
            var result = await service.CreateAsync(request.ToEntity(), request.LicenseImage);
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UploadLicenseImage(string id, [FromBody] UploadLicenseImageRequest request)
        {
            var result = await service.UpdateLicenseImageAsync(id, request.LicenseImage);
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
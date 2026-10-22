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
            var created = await service.CreateAsync(request.ToEntity(), request.LicenseImage);
            if (!created)
                return BadRequest(new MessageResponse(MessageResponse.InvalidData));

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UploadLicenseImage(string id, [FromBody] UploadLicenseImageRequest request)
        {
            var uploaded = await service.UpdateLicenseImageAsync(id, request.LicenseImage);
            if (!uploaded)
                return BadRequest(new MessageResponse(MessageResponse.InvalidData));

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}



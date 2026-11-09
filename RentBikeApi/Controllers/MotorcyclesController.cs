using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Motorcycles;
using RentBikeApi.Domain.Errors;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Mapping;

namespace RentBikeApi.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotorcyclesController(IMotorcycleService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request)
        {
            var result = await service.CreateAsync(request.ToEntity());
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? placa)
        {
            var motorcycles = await service.GetAllAsync(placa);
            return Ok(motorcycles.Select(m => m.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var motorcycle = await service.GetByIdAsync(id);
            if (motorcycle is null)
                return NotFound(new MessageResponse(MotorcycleErrors.NotFound.Description));
            return Ok(motorcycle.ToResponse());
        }

        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdatePlate(string id, [FromBody] UpdatePlateRequest request)
        {
            var result = await service.UpdatePlateAsync(id, request.Plate);
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));
            return Ok(new MessageResponse("Placa modificada com sucesso"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await service.DeleteAsync(id);
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));
            return Ok();
        }
    }
}
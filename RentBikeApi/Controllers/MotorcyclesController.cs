using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Motorcycles;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Mappings;

namespace RentBikeApi.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotorcyclesController(IMotorcycleService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request)
        {
            var created = await service.CreateAsync(request.ToEntity());
            if (!created)
                return BadRequest(new MessageResponse("Dados inválidos"));

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
                return NotFound(new MessageResponse("Moto não encontrada"));
            return Ok(motorcycle.ToResponse());
        }
        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdatePlate(string id, [FromBody] UpdatePlateRequest request)
        {
            var updated = await service.UpdatePlateAsync(id, request.Plate);
            if (!updated)
                return BadRequest(new MessageResponse("Dados inválidos"));
            return Ok(new MessageResponse("Placa modificada com sucesso"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await service.DeleteAsync(id);
            if (!deleted)
                return BadRequest(new MessageResponse("Dados inválidos"));
            return Ok();
        }
    }

}

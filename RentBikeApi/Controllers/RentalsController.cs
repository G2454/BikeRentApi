using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Rentals;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Mapping;

namespace RentBikeApi.Controllers
{
    [ApiController]
    [Route("locacao")]
    public class RentalsController(IRentalService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalRequest request)
        {
            var rental = await service.CreateAsync(
                courierId: request.CourierId,
                motorcycleId: request.MotorcycleId,
                planDays: request.Plan,
                requestedStarDate: request.StartDate.ToDateOnly(),
                requestedEndDate: request.EndDate.ToDateOnly(),
                requestedExpectedEndDate: request.ExpectedEndDate.ToDateOnly());

            if (rental is null)
                return BadRequest(new MessageResponse(MessageResponse.InvalidData));

            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental.ToResponse());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var rental = await service.GetByIdAsync(id);
            if (rental is null)
                return NotFound(new MessageResponse("Locação não encontrada"));
            return Ok(rental.ToResponse());
        }

        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> Return(string id, [FromBody] ReturnRentalRequest request)
        {
            var returned = await service.ReturnAsync(id, DateOnly.FromDateTime(request.ReturnDate));

            if (!returned)
                return BadRequest(new MessageResponse(MessageResponse.InvalidData));

            return Ok(new MessageResponse("Data de devolução informada com sucesso"));
        }
    }
}

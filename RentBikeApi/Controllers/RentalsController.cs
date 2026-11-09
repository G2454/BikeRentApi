using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Rentals;
using RentBikeApi.Domain.Errors;
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
            var result = await service.CreateAsync(
                courierId: request.CourierId,
                motorcycleId: request.MotorcycleId,
                planDays: request.Plan,
                requestedStartDate: request.StartDate.ToDateOnly(),
                requestedEndDate: request.EndDate.ToDateOnly(),
                requestedExpectedEndDate: request.ExpectedEndDate.ToDateOnly());

            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));

            var rental = result.Value;
            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental.ToResponse());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var rental = await service.GetByIdAsync(id);
            if (rental is null)
                return NotFound(new MessageResponse(RentalErrors.NotFound.Description));
            return Ok(rental.ToResponse());
        }

        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> Return(string id, [FromBody] ReturnRentalRequest request)
        {
            var result = await service.ReturnAsync(id, DateOnly.FromDateTime(request.ReturnDate));
            if (result.IsFailure)
                return BadRequest(ErrorResponse.InvalidData(result.Error));

            return Ok(new MessageResponse("Data de devolução informada com sucesso"));
        }
    }
}
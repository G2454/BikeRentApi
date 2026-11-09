using Microsoft.Extensions.Logging;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Errors;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.ValueObjects;

namespace RentBikeApi.Domain.Services
{
    public class RentalService(
        IRentalRepository rentalRepository,
        ICourierRepository courierRepository,
        IMotorcycleRepository motorcycleRepository,
        ILogger<RentalService> logger) : IRentalService
    {
        public async Task<Result<Rental>> CreateAsync(
            string courierId,
            string motorcycleId,
            int planDays,
            DateOnly? requestedStartDate,
            DateOnly? requestedEndDate,
            DateOnly? requestedExpectedEndDate)
        {
            var plan = RentalPlan.FromDays(planDays);
            if (plan is null)
                return RentalErrors.InvalidPlan;

            var courier = await courierRepository.GetByIdAsync(courierId);
            if (courier is null)
                return CourierErrors.NotFound;

            if (!courier.CanRentMotorcycle())
                return RentalErrors.CourierNotEligible;

            if (await motorcycleRepository.GetByIdAsync(motorcycleId) is null)
                return MotorcycleErrors.NotFound;

            var now = DateTime.UtcNow;
            var startDate = DateOnly.FromDateTime(now).AddDays(1);
            var expectedEndDate = startDate.AddDays(plan.Days - 1);

            if (requestedStartDate is not null && requestedStartDate != startDate)
                return RentalErrors.InvalidStartDate(startDate);

            if (requestedEndDate is not null && requestedEndDate != expectedEndDate)
                return RentalErrors.InvalidEndDate(expectedEndDate);

            if (requestedExpectedEndDate is not null && requestedExpectedEndDate != expectedEndDate)
                return RentalErrors.InvalidExpectedEndDate(expectedEndDate);

            if (await rentalRepository.MotorcycleHasActiveRentalAsync(motorcycleId, startDate))
                return RentalErrors.MotorcycleUnavailable;

            if (await rentalRepository.CourierHasActiveRentalAsync(courierId, startDate))
                return RentalErrors.CourierUnavailable;

            var rental = new Rental
            {
                Id = Guid.NewGuid().ToString(),
                CourierId = courierId,
                MotorcycleId = motorcycleId,
                PlanDays = plan.Days,
                DailyRate = plan.DailyRate,
                CreatedAt = now,
                StartDate = startDate,
                EndDate = expectedEndDate,
                ExpectedEndDate = expectedEndDate
            };
            await rentalRepository.AddAsync(rental);
            await rentalRepository.SaveChangesAsync();

            logger.LogInformation(
                "Rental {RentalId} created: courier {CourierId}, motorcycle {MotorcycleId}, {PlanDays}-day plan from {StartDate} to {ExpectedEndDate}",
                rental.Id, courierId, motorcycleId, plan.Days, startDate, expectedEndDate);

            return rental;
        }

        public Task<Rental?> GetByIdAsync(string id)
            => rentalRepository.GetByIdAsync(id);

        public async Task<Result> ReturnAsync(string id, DateOnly returnDate)
        {
            var rental = await rentalRepository.GetByIdAsync(id);
            if (rental is null)
                return RentalErrors.NotFound;

            var result = rental.RegisterReturn(returnDate);
            if (result.IsFailure)
                return result;

            await rentalRepository.SaveChangesAsync();

            logger.LogInformation("Rental {RentalId} returned on {ReturnDate}, total cost {TotalCost}", id, returnDate, rental.TotalCost);
            return Result.Success();
        }
    }
}
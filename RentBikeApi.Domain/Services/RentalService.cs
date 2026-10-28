using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.ValueObjects;

namespace RentBikeApi.Domain.Services
{
    public class RentalService(IRentalRepository rentalRepository, ICourierRepository courierRepository, IMotorcycleRepository motorcycleRepository) : IRentalService
    {
        public async Task<Rental?> CreateAsync(
            string courierId,
            string motorcycleId,
            int planDays,
            DateOnly? requestedStartDate,
            DateOnly? requestedEndDate,
            DateOnly? requestedExpectedEndDate)
        {
            var plan = RentalPlan.FromDays(planDays);
            if (plan is null)
                return null;

            var courier = await courierRepository.GetByIdAsync(courierId);
            if (courier is null || !courier.CanRentMotorcycle())
                return null;

            if (await motorcycleRepository.GetByIdAsync(motorcycleId) is null)
                return null;

            var now = DateTime.UtcNow;
            var startDate = DateOnly.FromDateTime(now).AddDays(1);
            var expectedEndDate = startDate.AddDays(plan.Days - 1);

            if (requestedStartDate is not null && requestedStartDate != startDate)
                return null;

            if (requestedEndDate is not null && requestedEndDate != expectedEndDate)
                return null;

            if (requestedExpectedEndDate is not null && requestedExpectedEndDate != expectedEndDate)
                return null;

            if (await rentalRepository.MotorcycleHasActiveRentalAsync(motorcycleId, startDate))
                return null;

            if (await rentalRepository.CourierHasActiveRentalAsync(courierId, startDate))
                return null;

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
            return rental;
        }

        public Task<Rental?> GetByIdAsync(string id)
            => rentalRepository.GetByIdAsync(id);

        public async Task<bool> ReturnAsync(string id, DateOnly returnDate)
        {
            var rental = await rentalRepository.GetByIdAsync(id);
            if (rental is null)
                return false;

            if (!rental.RegisterReturn(returnDate))
                return false;

            await rentalRepository.SaveChangesAsync();
            return true;

        }

    }
}

using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface IRentalService
    {
        Task<Rental?> CreateAsync(
            string courierId,
            string motorcycleId,
            int planDays,
            DateOnly? requestedStarDate,
            DateOnly? requestedEndDate,
            DateOnly? requestedExpectedEndDate
            );

        Task<Rental?> GetByIdAsync(string id);
        Task<bool> ReturnAsync(string id, DateOnly returnDate);
    }
}

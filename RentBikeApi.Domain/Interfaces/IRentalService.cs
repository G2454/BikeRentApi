using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface IRentalService
    {
        Task<Result<Rental>> CreateAsync(
            string courierId,
            string motorcycleId,
            int planDays,
            DateOnly? requestedStartDate,
            DateOnly? requestedEndDate,
            DateOnly? requestedExpectedEndDate
            );

        Task<Rental?> GetByIdAsync(string id);
        Task<Result> ReturnAsync(string id, DateOnly returnDate);
    }
}
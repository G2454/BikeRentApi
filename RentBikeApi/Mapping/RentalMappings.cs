using RentBikeApi.Contracts.Rentals;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Mapping
{
    public static class RentalMappings
    {
        public static RentalResponse ToResponse(this Rental rental) => new(
            rental.Id,
            rental.DailyRate,
            rental.CourierId,
            rental.MotorcycleId,
            ToUtcDateTime(rental.StartDate),
            ToUtcDateTime(rental.EndDate),
            ToUtcDateTime(rental.ExpectedEndDate),
            rental.ReturnDate?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            rental.TotalCost);

        private static DateTime ToUtcDateTime(DateOnly date)
            => date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
    }
}

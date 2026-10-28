using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<Rental?> GetByIdAsync(string id);
        Task<bool> ExistsForMotorcycleAsync(string motorcycleId);
        Task<bool> MotorcycleHasActiveRentalAsync(string motorcycleId, DateOnly date);
        Task<bool> CourierHasActiveRentalAsync(string courierId, DateOnly date);
        Task AddAsync(Rental rental);
        Task SaveChangesAsync();
    }
}

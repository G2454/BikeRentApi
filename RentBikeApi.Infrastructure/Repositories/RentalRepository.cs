using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Persistence;

namespace RentBikeApi.Infrastructure.Repositories
{
    public class RentalRepository(AppDbContext db) : IRentalRepository
    {
        public async Task<Rental?> GetByIdAsync(string id)
            => await db.Rentals.FindAsync(id);

        public Task<bool> ExistsForMotorcycleAsync(string motorcycleId)
            => db.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId);

        public Task<bool> MotorcycleHasActiveRentalAsync(string motorcycleId, DateOnly date)
            => db.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId &&
            (r.ReturnDate == null || r.ReturnDate >= date));

        public Task<bool> CourierHasActiveRentalAsync(string courierId, DateOnly date)
            => db.Rentals.AnyAsync(r => r.CourierId == courierId &&
            (r.ReturnDate == null || r.ReturnDate >= date));

        public async Task AddAsync(Rental rental)
            => await db.Rentals.AddAsync(rental);

        public Task SaveChangesAsync()
            => db.SaveChangesAsync();
    }
}

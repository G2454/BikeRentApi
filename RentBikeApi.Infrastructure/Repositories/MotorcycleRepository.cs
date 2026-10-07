using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Persistence;


namespace RentBikeApi.Infrastructure.Repositories
{
    public class MotorcycleRepository(AppDbContext db): IMotorcycleRepository
    {
        public async Task<Motorcycle?> GetByIdAsync(string id)
            => await db.Motorcycles.FindAsync(id);

        public async Task<List<Motorcycle>> GetAllAsync(string? plate)
        {
            var query = db.Motorcycles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(plate))
                query = query.Where(m => m.Plate == plate);
            return await query.ToListAsync();
        }

        public Task<bool> PlateExistsAsync(string plate)
            => db.Motorcycles.AnyAsync(m => m.Plate == plate);

        public async Task AddAsync(Motorcycle motorcycle)
            => await db.Motorcycles.AddAsync(motorcycle);

        public void Remove(Motorcycle motorcycle)
            => db.Motorcycles.Remove(motorcycle);

        public Task SaveChangesAsync()
            => db.SaveChangesAsync();
    }
}

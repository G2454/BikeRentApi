using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Persistence;

namespace RentBikeApi.Infrastructure.Repositories
{
    public class CourierRepository(AppDbContext db) : ICourierRepository
    {
        public async Task<Courier?> GetByIdAsync(string id)
            => await db.Couriers.FindAsync(id);

        public Task<bool> CnpjExistsAsync(string cnpj)
            => db.Couriers.AnyAsync(c => c.Cnpj == cnpj);

        public Task<bool> LicenseNumberExistsAsync(string licenseNumber)
            => db.Couriers.AnyAsync(c => c.LicenseNumber == licenseNumber);

        public async Task AddAsync(Courier courier)
            => await db.Couriers.AddAsync(courier);

        public Task SaveChangesAsync()
            => db.SaveChangesAsync();
    }
}

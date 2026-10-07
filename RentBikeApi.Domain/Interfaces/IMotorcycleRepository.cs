using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces;

public interface IMotorcycleRepository
{
    Task<Motorcycle?> GetByIdAsync(string id);
    Task<List<Motorcycle>> GetAllAsync(string? plate);
    Task<bool> PlateExistsAsync(string plate);
    Task AddAsync(Motorcycle motorcycle);
    void Remove(Motorcycle motorcycle);
    Task SaveChangesAsync();
    
}
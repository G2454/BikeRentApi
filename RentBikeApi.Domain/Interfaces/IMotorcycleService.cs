using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces;

public interface IMotorcycleService
{
    Task<bool> CreateAsync(Motorcycle motorcycle);
    Task<List<Motorcycle>> GetAllAsync(string? plate);
    Task<Motorcycle?> GetByIdAsync(string id);
    Task<bool> UpdatePlateAsync(string id, string newPlate);
    Task<bool> DeleteAsync(string id);

}
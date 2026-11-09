using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Common;

namespace RentBikeApi.Domain.Interfaces;

public interface IMotorcycleService
{
    Task<Result> CreateAsync(Motorcycle motorcycle);
    Task<List<Motorcycle>> GetAllAsync(string? plate);
    Task<Motorcycle?> GetByIdAsync(string id);
    Task<Result> UpdatePlateAsync(string id, string newPlate);
    Task<Result> DeleteAsync(string id);

}
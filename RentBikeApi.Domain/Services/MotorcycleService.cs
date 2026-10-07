using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Domain.Services;

public class MotorcycleService(IMotorcycleRepository repository): IMotorcycleService
{
    public async Task<bool> CreateAsync(Motorcycle motorcycle)
    {
        if (string.IsNullOrWhiteSpace(motorcycle.Id) ||
           string.IsNullOrWhiteSpace(motorcycle.Model) ||
           string.IsNullOrWhiteSpace(motorcycle.Plate) ||
           motorcycle.Year <= 0)
            return false;

        if (await repository.PlateExistsAsync(motorcycle.Plate))
            return false;

        if (await repository.GetByIdAsync(motorcycle.Id) is not null)
            return false;

        await repository.AddAsync(motorcycle);
        await repository.SaveChangesAsync();
        return true;
    }

    public Task<List<Motorcycle>> GetAllAsync(string? plate)
        => repository.GetAllAsync(plate);

    public Task<Motorcycle?> GetByIdAsync(string id)
        => repository.GetByIdAsync(id);

    public async Task<bool> UpdatePlateAsync(string id, string newPlate)
    {
        if (string.IsNullOrWhiteSpace(newPlate))
            return false;

        var motorcycle = await repository.GetByIdAsync(id);
        if (motorcycle is null)
            return false;

        if (await repository.PlateExistsAsync(newPlate))
            return false;

        motorcycle.Plate = newPlate;
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var motorcycle = await repository.GetByIdAsync(id);
        if (motorcycle is null)
            return false;

        //TODO: RETURN FALSE IF THIS MOTORCYCLE HAS RENTALS


        repository.Remove(motorcycle);
        await repository.SaveChangesAsync();
        return true;
    }
}
using Microsoft.Extensions.Logging;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Errors;
using RentBikeApi.Domain.Events;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Domain.Services;

public class MotorcycleService(
    IMotorcycleRepository repository,
    IEventPublisher eventPublisher,
    IRentalRepository rentalRepository,
    ILogger<MotorcycleService> logger) : IMotorcycleService
{
    public async Task<Result> CreateAsync(Motorcycle motorcycle)
    {
        motorcycle.Plate = NormalizePlate(motorcycle.Plate);

        if (string.IsNullOrWhiteSpace(motorcycle.Id))
            return MotorcycleErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(motorcycle.Model))
            return MotorcycleErrors.ModelRequired;

        if (motorcycle.Model.Length > Motorcycle.ModelMaxLength)
            return MotorcycleErrors.ModelTooLong;

        var plateError = ValidatePlate(motorcycle.Plate);
        if (plateError is not null)
            return plateError;

        if (motorcycle.Year <= 0)
            return MotorcycleErrors.InvalidYear;

        if (await repository.GetByIdAsync(motorcycle.Id) is not null)
            return MotorcycleErrors.IdInUse;

        if (await repository.PlateExistsAsync(motorcycle.Plate))
            return MotorcycleErrors.PlateInUse;

        await repository.AddAsync(motorcycle);
        await repository.SaveChangesAsync();

        logger.LogInformation("Motorcycle {MotorcycleId} registered with plate {Plate}", motorcycle.Id, motorcycle.Plate);

        await eventPublisher.PublishAsync(new MotorcycleRegisteredEvent(
            motorcycle.Id,
            motorcycle.Year,
            motorcycle.Model,
            motorcycle.Plate,
            DateTime.UtcNow
            ));

        return Result.Success();
    }

    public Task<List<Motorcycle>> GetAllAsync(string? plate)
        => repository.GetAllAsync(string.IsNullOrWhiteSpace(plate) ? null : NormalizePlate(plate));

    public Task<Motorcycle?> GetByIdAsync(string id)
        => repository.GetByIdAsync(id);

    public async Task<Result> UpdatePlateAsync(string id, string newPlate)
    {
        newPlate = NormalizePlate(newPlate);

        var plateError = ValidatePlate(newPlate);
        if (plateError is not null)
            return plateError;

        var motorcycle = await repository.GetByIdAsync(id);
        if (motorcycle is null)
            return MotorcycleErrors.NotFound;

        if (await repository.PlateExistsAsync(newPlate))
            return MotorcycleErrors.PlateInUse;

        var previousPlate = motorcycle.Plate;
        motorcycle.Plate = newPlate;
        await repository.SaveChangesAsync();

        logger.LogInformation("Motorcycle {MotorcycleId} plate changed from {PreviousPlate} to {Plate}", id, previousPlate, newPlate);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var motorcycle = await repository.GetByIdAsync(id);
        if (motorcycle is null)
            return MotorcycleErrors.NotFound;

        if (await rentalRepository.ExistsForMotorcycleAsync(id))
            return MotorcycleErrors.HasRentals;

        repository.Remove(motorcycle);
        await repository.SaveChangesAsync();

        logger.LogInformation("Motorcycle {MotorcycleId} deleted", id);
        return Result.Success();
    }

    private static string NormalizePlate(string? plate)
        => (plate ?? string.Empty).Trim().ToUpperInvariant();

    private static Error? ValidatePlate(string normalizedPlate)
    {
        if (normalizedPlate.Length == 0)
            return MotorcycleErrors.PlateRequired;

        if (normalizedPlate.Length > Motorcycle.PlateMaxLength)
            return MotorcycleErrors.PlateTooLong;

        return null;
    }
}
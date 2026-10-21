using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Events;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Domain.Services;

public class MotorcycleNotificationService(IMotorcycleNotificationRepository repository) : IMotorcycleNotificationService
{
    private const int NotifiableYear = 2024;

    public async Task<bool> HandleMotorcycleRegisteredAsync(MotorcycleRegisteredEvent @event)
    {
        if (@event.Year != NotifiableYear)
            return false;

        var notification = new MotorcycleNotification
        {
            Id = Guid.NewGuid(),
            MotorcycleId = @event.MotorcycleId,
            Year = @event.Year,
            Model = @event.Model,
            Plate = @event.Plate,
            ReceivedAt = DateTime.UtcNow
        };

        await repository.AddAsync(notification);
        await repository.SaveChangesAsync();
        return true;
    }
}
using RentBikeApi.Domain.Events;

namespace RentBikeApi.Domain.Interfaces;

public interface IMotorcycleNotificationService
{
    Task<bool> HandleMotorcycleRegisteredAsync(MotorcycleRegisteredEvent @event);
}
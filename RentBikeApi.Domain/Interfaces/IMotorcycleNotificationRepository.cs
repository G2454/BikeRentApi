using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces;

public interface IMotorcycleNotificationRepository
{
    Task AddAsync(MotorcycleNotification notification);
    Task SaveChangesAsync();
}
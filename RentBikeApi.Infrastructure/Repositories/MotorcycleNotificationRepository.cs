using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Persistence;


namespace RentBikeApi.Infrastructure.Repositories
{
    public class MotorcycleNotificationRepository(AppDbContext db): IMotorcycleNotificationRepository
    {
        public async Task AddAsync(MotorcycleNotification notification) => await db.MotorcycleNotifications.AddAsync(notification);

        public Task SaveChangesAsync() => db.SaveChangesAsync();
    }
}

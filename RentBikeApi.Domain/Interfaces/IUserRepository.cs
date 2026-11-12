using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}
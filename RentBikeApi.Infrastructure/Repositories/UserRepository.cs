using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Persistence;

namespace RentBikeApi.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public Task<User?> GetByUsernameAsync(string username)
            => db.Users.FirstOrDefaultAsync(u => u.Username == username);

        public Task<bool> UsernameExistsAsync(string username)
            => db.Users.AnyAsync(u => u.Username == username);

        public async Task AddAsync(User user)
            => await db.Users.AddAsync(user);

        public Task SaveChangesAsync()
            => db.SaveChangesAsync();
    }
}
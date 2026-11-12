using Microsoft.AspNetCore.Identity;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Infrastructure.Auth
{
    public sealed class IdentityPasswordHasher : Domain.Interfaces.IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string Hash(string password)
            => _hasher.HashPassword(null!, password);

        public bool Verify(string passwordHash, string password)
            => _hasher.VerifyHashedPassword(null!, passwordHash, password) != PasswordVerificationResult.Failed;
    }
}
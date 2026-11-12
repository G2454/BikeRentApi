using Microsoft.Extensions.Logging;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Enums;
using RentBikeApi.Domain.Errors;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Domain.Services;

public class AuthService(
    IUserRepository repository,
    IPasswordHasher passwordHasher,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<User>> LoginAsync(string username, string password)
    {
        var user = string.IsNullOrWhiteSpace(username)
            ? null
            : await repository.GetByUsernameAsync(username);

        if (user is null || string.IsNullOrEmpty(password) || !passwordHasher.Verify(user.PasswordHash, password))
        {
            logger.LogWarning("Failed login for user {Username}", username);
            return AuthErrors.InvalidCredentials;
        }

        logger.LogInformation("User {Username} logged in as {Role}", user.Username, user.Role);
        return user;
    }

    public async Task EnsureAdminAsync(string username, string password)
    {
        if (await repository.UsernameExistsAsync(username))
            return;

        await repository.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHasher.Hash(password),
            Role = UserRole.Admin
        });
        await repository.SaveChangesAsync();

        logger.LogInformation("Admin user {Username} created", username);
    }
}
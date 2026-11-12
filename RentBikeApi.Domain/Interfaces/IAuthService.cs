using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces;

public interface IAuthService
{
    Task<Result<User>> LoginAsync(string username, string password);
    Task EnsureAdminAsync(string username, string password);
}
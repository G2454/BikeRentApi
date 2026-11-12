using RentBikeApi.Domain.Enums;

namespace RentBikeApi.Auth;

public static class Roles
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Courier = nameof(UserRole.Courier);
}

public static class AuthClaims
{
    public const string Name = "name";
    public const string Role = "role";
    public const string CourierId = "courier_id";
}
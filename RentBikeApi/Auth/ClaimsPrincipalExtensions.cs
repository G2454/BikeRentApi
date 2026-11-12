using System.Security.Claims;

namespace RentBikeApi.Auth;

public static class ClaimsPrincipalExtensions
{
    public static string? GetCourierId(this ClaimsPrincipal user)
        => user.FindFirstValue(AuthClaims.CourierId);
    public static bool CanActForCourier(this ClaimsPrincipal user, string courierId)
        => user.IsInRole(Roles.Admin) || user.GetCourierId() == courierId;
}
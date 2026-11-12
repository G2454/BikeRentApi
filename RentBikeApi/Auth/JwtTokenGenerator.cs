using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Auth;

public sealed record AccessToken(string Token, DateTime ExpiresAt);

public sealed class JwtTokenGenerator(IOptions<JwtOptions> options)
{
    public AccessToken Generate(User user)
    {
        var settings = options.Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes);

        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
            [AuthClaims.Name] = user.Username,
            [AuthClaims.Role] = user.Role.ToString()
        };
        if (user.CourierId is not null)
            claims[AuthClaims.CourierId] = user.CourierId;

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            Expires = expiresAt,
            Claims = claims,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey)),
                SecurityAlgorithms.HmacSha256)
        };

        return new AccessToken(new JsonWebTokenHandler().CreateToken(descriptor), expiresAt);
    }
}
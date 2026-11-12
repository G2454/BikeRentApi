using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Auth;

public record LoginResponse(
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("tipo")] string Type,
    [property: JsonPropertyName("expira_em")] DateTime ExpiresAt);
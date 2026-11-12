using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Auth;

public record LoginRequest(
    [property: JsonPropertyName("usuario")] string Username,
    [property: JsonPropertyName("senha")] string Password);
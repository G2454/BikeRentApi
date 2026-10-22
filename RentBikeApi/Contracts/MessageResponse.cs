using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts;

public record MessageResponse([property: JsonPropertyName("mensagem")] string Message)
{
    public const string InvalidData = "Dados inválidos";
}
using System.Text.Json.Serialization;
using RentBikeApi.Domain.Common;

namespace RentBikeApi.Contracts;

public record ErrorResponse(
    [property: JsonPropertyName("mensagem")] string Message,
    [property: JsonPropertyName("codigo")] string Code,
    [property: JsonPropertyName("detalhe")] string Detail)
{
    public static ErrorResponse InvalidData(Error error)
        => new(MessageResponse.InvalidData, error.Code, error.Description);
}
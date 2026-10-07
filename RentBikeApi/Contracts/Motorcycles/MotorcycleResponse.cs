using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Motorcycles;

public record MotorcycleResponse(
    [property: JsonPropertyName("identificador")] string Id,
    [property: JsonPropertyName("ano")] int Year,
    [property: JsonPropertyName("modelo")] string Model,
    [property: JsonPropertyName("placa")] string Plate
    );
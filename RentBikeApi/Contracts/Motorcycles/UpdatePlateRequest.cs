using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Motorcycles;

public record UpdatePlateRequest(
    [property: JsonPropertyName("placa")] string Plate
    );
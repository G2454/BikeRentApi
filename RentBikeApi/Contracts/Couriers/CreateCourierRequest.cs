using System.Text.Json.Serialization;
using RentBikeApi.Converters;
using RentBikeApi.Domain.Enums; 

namespace RentBikeApi.Contracts.Couriers
{
    public record CreateCourierRequest(
        [property: JsonPropertyName("identificador")] string Id,
        [property: JsonPropertyName("nome")] string Name,
        [property: JsonPropertyName("cnpj")] string Cnpj,
        [property: JsonPropertyName("data_nascimento")] DateTime BirthDate,
        [property: JsonPropertyName("numero_cnh")] string LicenseNumber,
        [property: JsonPropertyName("tipo_cnh"), JsonConverter(typeof(LicenseTypeJsonConverter))] LicenseType LicenseType,
        [property: JsonPropertyName("imagem_cnh")] string? LicenseImage);
   
}

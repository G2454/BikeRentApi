using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Rentals
{
    public record CreateRentalRequest(
        [property: JsonPropertyName("entregador_id")] string CourierId,
        [property: JsonPropertyName("moto_id")] string MotorcycleId,
        [property: JsonPropertyName("data_inicio")] DateTime? StartDate,
        [property: JsonPropertyName("data_termino")] DateTime? EndDate,
        [property: JsonPropertyName("data_previsao_termino")] DateTime? ExpectedEndDate,
        [property: JsonPropertyName("plano")] int Plan
        );
}

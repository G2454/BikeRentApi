using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Rentals
{
    public record RentalResponse(
        [property: JsonPropertyName("identificador")] string Id,
        [property: JsonPropertyName("valor_diaria")] decimal DailyRate,
        [property: JsonPropertyName("entregador_id")] string CourierId,
        [property: JsonPropertyName("moto_id")] string MotorcycleId,
        [property: JsonPropertyName("data_inicio")] DateTime StartDate,
        [property: JsonPropertyName("data_termino")] DateTime EndDate,
        [property: JsonPropertyName("data_previsao_termino")] DateTime ExpectedEndDate,
        [property: JsonPropertyName("data_devolucao")] DateTime? ReturnDate,
        [property: JsonPropertyName("valor_total")] decimal? TotalCost
        );
}

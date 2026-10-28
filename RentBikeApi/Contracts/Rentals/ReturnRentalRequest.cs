using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Rentals
{
    public record ReturnRentalRequest(
        [property: JsonPropertyName("data_devolucao")] DateTime ReturnDate
        );
}

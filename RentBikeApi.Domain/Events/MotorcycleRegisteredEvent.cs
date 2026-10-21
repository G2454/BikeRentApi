namespace RentBikeApi.Domain.Events
{
    public record MotorcycleRegisteredEvent(
        string MotorcycleId,
        int Year,
        string Model,
        string Plate,
        DateTime OccurredAt
        );
}

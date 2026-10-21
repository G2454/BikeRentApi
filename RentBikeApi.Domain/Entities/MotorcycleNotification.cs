namespace RentBikeApi.Domain.Entities;

public class MotorcycleNotification
{
    public Guid Id { get; set; }
    public string MotorcycleId { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}


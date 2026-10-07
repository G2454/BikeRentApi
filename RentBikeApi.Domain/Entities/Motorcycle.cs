namespace RentBikeApi.Domain.Entities;

public class Motorcycle
{
    public string Id { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
}


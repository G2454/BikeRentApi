namespace RentBikeApi.Domain.Entities;

public class Motorcycle
{

    public const int ModelMaxLength = 100;
    public const int PlateMaxLength = 12;

    public string Id { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
}


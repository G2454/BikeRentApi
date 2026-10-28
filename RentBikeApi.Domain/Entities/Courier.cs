using RentBikeApi.Domain.Enums;

namespace RentBikeApi.Domain.Entities
{
    public class Courier
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public LicenseType LicenseType { get; set; }
        public string? LicenseImagePath { get; set; }

        public bool CanRentMotorcycle() => LicenseType is LicenseType.A or LicenseType.AB;
    }
}

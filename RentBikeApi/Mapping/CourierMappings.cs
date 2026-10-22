using RentBikeApi.Contracts.Couriers;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Mapping;

public static class CourierMappings
{
    public static Courier ToEntity(this CreateCourierRequest request) => new()
    {
        Id = request.Id,
        Name = request.Name,
        Cnpj = request.Cnpj,
        BirthDate = DateOnly.FromDateTime(request.BirthDate),
        LicenseNumber = request.LicenseNumber,
        LicenseType = request.LicenseType
    };
}

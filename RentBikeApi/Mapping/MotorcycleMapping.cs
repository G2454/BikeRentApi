using RentBikeApi.Contracts.Motorcycles;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Mappings;

public static class MotorcycleMappings
{
    public static Motorcycle ToEntity(this CreateMotorcycleRequest request) => new()
    {
        Id = request.Id,
        Year = request.Year,
        Model = request.Model,
        Plate = request.Plate
    };

    public static MotorcycleResponse ToResponse(this Motorcycle motorcycle)
        => new(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
}
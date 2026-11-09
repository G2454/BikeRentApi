using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Errors;

public static class MotorcycleErrors
{
    public static readonly Error IdRequired =
        new("motorcycle.id_required", "O identificador da moto é obrigatório");

    public static readonly Error ModelRequired =
        new("motorcycle.model_required", "O modelo da moto é obrigatório");

    public static readonly Error ModelTooLong =
        new("motorcycle.model_too_long", $"O modelo da moto deve ter no máximo {Motorcycle.ModelMaxLength} caracteres");

    public static readonly Error PlateRequired =
        new("motorcycle.plate_required", "A placa da moto é obrigatória");

    public static readonly Error PlateTooLong =
        new("motorcycle.plate_too_long", $"A placa da moto deve ter no máximo {Motorcycle.PlateMaxLength} caracteres");

    public static readonly Error InvalidYear =
        new("motorcycle.invalid_year", "O ano da moto é inválido");

    public static readonly Error IdInUse =
        new("motorcycle.id_in_use", "Já existe uma moto com este identificador");

    public static readonly Error PlateInUse =
        new("motorcycle.plate_in_use", "Já existe uma moto com esta placa");

    public static readonly Error NotFound =
        new("motorcycle.not_found", "Moto não encontrada");

    public static readonly Error HasRentals =
        new("motorcycle.has_rentals", "A moto possui locações registradas e não pode ser removida");
}
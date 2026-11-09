using Microsoft.EntityFrameworkCore;
using Npgsql;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Errors;

namespace RentBikeApi.Infrastructure.Persistence;
public static class DatabaseErrorTranslator
{
    public const string OneOpenRentalPerMotorcycle = "ix_rentals_one_open_per_motorcycle";
    public const string OneOpenRentalPerCourier = "ix_rentals_one_open_per_courier";

    public static Error? Translate(DbUpdateException exception)
    {
        if (exception.InnerException is not PostgresException postgres)
            return null;

        return postgres.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation => postgres.ConstraintName switch
            {
                "pk_motorcycles" => MotorcycleErrors.IdInUse,
                "ix_motorcycles_plate" => MotorcycleErrors.PlateInUse,
                "pk_couriers" => CourierErrors.IdInUse,
                "ix_couriers_cnpj" => CourierErrors.CnpjInUse,
                "ix_couriers_license_number" => CourierErrors.LicenseNumberInUse,
                OneOpenRentalPerMotorcycle => RentalErrors.MotorcycleUnavailable,
                OneOpenRentalPerCourier => RentalErrors.CourierUnavailable,
                _ => null
            },

            PostgresErrorCodes.ForeignKeyViolation => postgres.ConstraintName switch
            {
                "fk_rentals_motorcycles_motorcycle_id" when IsDeleting<Motorcycle>(exception) => MotorcycleErrors.HasRentals,
                "fk_rentals_motorcycles_motorcycle_id" => MotorcycleErrors.NotFound,
                "fk_rentals_couriers_courier_id" => CourierErrors.NotFound,
                _ => null
            },

            _ => null
        };
    }

    private static bool IsDeleting<TEntity>(DbUpdateException exception)
        => exception.Entries.Any(entry => entry.Entity is TEntity && entry.State == EntityState.Deleted);
}
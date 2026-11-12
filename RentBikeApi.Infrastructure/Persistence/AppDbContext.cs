using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<MotorcycleNotification> MotorcycleNotifications => Set<MotorcycleNotification>();
    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<Rental> Rentals => Set<Rental>();

    public DbSet<User> Users => Set<User>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (DatabaseErrorTranslator.Translate(exception) is { } error)
        {
            throw new DomainException(error, exception);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.Plate).IsUnique();
            entity.Property(m => m.Model).IsRequired();
            entity.Property(m => m.Plate).IsRequired();
            entity.Property(m => m.Model).HasMaxLength(Motorcycle.ModelMaxLength);
            entity.Property(m => m.Plate).HasMaxLength(Motorcycle.PlateMaxLength);
        });

        modelBuilder.Entity<MotorcycleNotification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.MotorcycleId).IsRequired().HasMaxLength(100);
            entity.Property(n => n.Model).IsRequired().HasMaxLength(Motorcycle.ModelMaxLength);
            entity.Property(n => n.Plate).IsRequired().HasMaxLength(Motorcycle.PlateMaxLength);
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Cnpj).IsUnique();
            entity.HasIndex(c => c.LicenseNumber).IsUnique();

            entity.Property(c => c.Name).IsRequired().HasMaxLength(Courier.NameMaxLength);
            entity.Property(c => c.Cnpj).IsRequired().HasMaxLength(14);
            entity.Property(c => c.LicenseNumber).IsRequired().HasMaxLength(11);
            entity.Property(c => c.LicenseType).HasConversion<string>().HasMaxLength(2);
            entity.Property(c => c.LicenseImagePath).HasMaxLength(500);
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.DailyRate).HasPrecision(10, 2);
            entity.Property(r => r.TotalCost).HasPrecision(10, 2);

            entity.HasOne<Courier>()
            .WithMany()
            .HasForeignKey(r => r.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Motorcycle>()
            .WithMany()
            .HasForeignKey(r => r.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(r => r.MotorcycleId, "ix_rentals_one_open_per_motorcycle")
            .HasDatabaseName(DatabaseErrorTranslator.OneOpenRentalPerMotorcycle)
            .IsUnique()
            .HasFilter("return_date IS NULL");

            entity.HasIndex(r => r.CourierId, "ix_rentals_one_open_per_courier")
            .HasDatabaseName(DatabaseErrorTranslator.OneOpenRentalPerCourier)
            .IsUnique()
            .HasFilter("return_date IS NULL");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);


            entity.HasIndex(u => u.CourierId).IsUnique();
            entity.HasOne<Courier>()
            .WithMany()
            .HasForeignKey(u => u.CourierId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
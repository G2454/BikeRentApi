using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<MotorcycleNotification> MotorcycleNotifications => Set<MotorcycleNotification>();
    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<Rental> Rentals => Set<Rental>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.Plate).IsUnique();
            entity.Property(m => m.Model).IsRequired();
            entity.Property(m => m.Plate).IsRequired();
            entity.Property(m => m.Model).HasMaxLength(100);
            entity.Property(m => m.Plate).HasMaxLength(12);
        });

        modelBuilder.Entity<MotorcycleNotification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.MotorcycleId).IsRequired().HasMaxLength(100);
            entity.Property(n => n.Model).IsRequired().HasMaxLength(100);
            entity.Property(n => n.Plate).IsRequired().HasMaxLength(12);
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Cnpj).IsUnique();
            entity.HasIndex(c => c.LicenseNumber).IsUnique();

            entity.Property(c => c.Name).IsRequired().HasMaxLength(150);
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
            .IsUnique()
            .HasFilter("return_date IS NULL");

            entity.HasIndex(r => r.CourierId, "ix_rentals_one_open_per_courier")
            .IsUnique()
            .HasFilter("return_date IS NULL");
        });
    }
}

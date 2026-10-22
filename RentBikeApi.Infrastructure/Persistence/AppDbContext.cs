using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<MotorcycleNotification> MotorcycleNotifications => Set<MotorcycleNotification>();

    public DbSet<Courier> Couriers => Set<Courier>();

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
            entity.Property(c => c.Cnpj).IsRequired().HasMaxLength(20);
            entity.Property(c => c.LicenseNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.LicenseType).HasConversion<string>().HasMaxLength(2);
            entity.Property(c => c.LicenseImagePath).HasMaxLength(500);
        });
    }
}

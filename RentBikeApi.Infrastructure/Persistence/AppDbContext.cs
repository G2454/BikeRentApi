using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<MotorcycleNotification> MotorcycleNotifications => Set<MotorcycleNotification>();

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


    }
}

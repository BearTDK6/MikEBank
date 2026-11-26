using Microsoft.EntityFrameworkCore;
using MikEBank.Api.Models;

namespace MikEBank.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Role)
                .HasConversion(
                    role => role.ToString().ToLowerInvariant(),
                    value => Enum.Parse<UserRole>(value, true))
                .HasMaxLength(20)
                .HasDefaultValue(UserRole.Customer)
                .IsRequired();

            entity.Property(user => user.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.IdentityNumber).HasMaxLength(12).IsRequired();
            entity.HasIndex(user => user.IdentityNumber).IsUnique();
            entity.Property(user => user.Gender).HasMaxLength(20).IsRequired();
            entity.Property(user => user.Nationality).HasMaxLength(100).IsRequired();
            entity.Property(user => user.PlaceOfOrigin).HasMaxLength(250).IsRequired();
            entity.Property(user => user.PlaceOfResidence).HasMaxLength(250).IsRequired();

            entity.Property(user => user.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.Property(user => user.Phone)
                .HasMaxLength(20);
        });
    }
}
using ColorsApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Database;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("auth");

        builder.Entity<RefreshTokenEntity>()
            .HasKey(refreshToken => refreshToken.Id);

        builder.Entity<RefreshTokenEntity>()
            .HasIndex(builder => builder.Token)
            .IsUnique();

        builder.Entity<RefreshTokenEntity>()
            .HasOne(builder => builder.User)
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId);

    }
}

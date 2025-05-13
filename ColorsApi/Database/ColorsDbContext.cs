using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Database;

public class ColorsDbContext : DbContext
{
    public ColorsDbContext(DbContextOptions<ColorsDbContext> options) : base(options) { }
    public DbSet<ColorEntity> Colors { get; set; }
    public DbSet<PaletteEntity> Palettes { get; set; }
    public DbSet<ColorsUserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColorsDbContext).Assembly);
    }
}

using ColorsApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ColorsApi.Database;

public class ColorsDbContext : DbContext
{
    public ColorsDbContext(DbContextOptions<ColorsDbContext> options) : base(options) { }
    public DbSet<ColorEntity> Colors { get; set; }
    public DbSet<PaletteEntity> Palettes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColorsDbContext).Assembly);
    }
}

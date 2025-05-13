using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorsApi.Database;

public class PalettesEntityConfiguration : IEntityTypeConfiguration<PaletteEntity>
{
    public void Configure(EntityTypeBuilder<PaletteEntity> builder)
    {
        builder.HasKey(palette => palette.Id);
        builder.HasMany(palette => palette.Colors)
            .WithOne()
            .HasForeignKey(color => color.PaletteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

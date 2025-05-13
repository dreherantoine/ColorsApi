using ColorsApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorsApi.Database;

public class ColorsEntityConfiguration : IEntityTypeConfiguration<ColorEntity>
{
    public void Configure(EntityTypeBuilder<ColorEntity> builder)
    {
        builder.HasKey(color => color.Id);
        builder.Property(color => color.PaletteId).IsRequired();
        builder.Property(color => color.Type).IsRequired();
        builder.Property(color => color.Red).IsRequired();
        builder.Property(color => color.Green).IsRequired();
        builder.Property(color => color.Blue).IsRequired();
    }
}

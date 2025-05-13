namespace ColorsApi.Entities;

public class PaletteEntity
{
    public int Id { get; set; }
    public IReadOnlyCollection<ColorEntity> Colors { get; set; }
    public bool IsArchived { get; set; }
}

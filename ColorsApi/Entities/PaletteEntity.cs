namespace ColorsApi.Entities;

public class PaletteEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public IReadOnlyCollection<ColorEntity> Colors { get; set; }
    public bool IsArchived { get; set; }
}

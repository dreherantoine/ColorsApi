namespace ColorsApi.Entities;

public class ColorEntity
{
    public int Id { get; set; }
    public int PaletteId { get; set; }
    public PaletteEntity Palette { get; set; }
    public int Type { get; set; }
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public bool IsArchived { get; set; }
}

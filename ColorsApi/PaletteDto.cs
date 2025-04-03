namespace ColorsApi;

public class PaletteDto
{
    public IReadOnlyCollection<ColorDto> Colors { get; set; }

    public static PaletteDto RandomPalette()
    {
        var random = new Random();
        var palette = new PaletteDto
        {
            Colors = Enumerable.Range(0, 5).Select(index => new ColorDto
            {
                Type = index,
                Red = (byte)random.Next(0, 255),
                Green = (byte)random.Next(0, 255),
                Blue = (byte)random.Next(0, 255),
            }).ToArray()
        };

        return palette;
    }
}

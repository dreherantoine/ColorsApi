﻿using System.Text.Json.Serialization;

namespace ColorsApi.Models;

public class Palette
{
    [JsonRequired]
    public IReadOnlyCollection<ColorCode> Colors { get; set; }

    public static Palette RandomPalette()
    {
        var random = new Random();
        var palette = new Palette
        {
            Colors = Enumerable.Range(0, 5).Select(index => new ColorCode
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

public class PaletteEntity
{
    public int Id { get; set; }
    public IReadOnlyCollection<ColorEntity> Colors { get; set; }
    public bool IsArchived { get; set; }
}

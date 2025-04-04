using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ColorsApi.Models;

public class ColorCode
{
    [JsonRequired]
    public int Type { get; set; }
    [JsonRequired]
    public byte Red { get; set; }
    [JsonRequired]
    public byte Green { get; set; }
    [JsonRequired]
    public byte Blue { get; set; }
}

public class ColorEntity
{
    public int Id { get; set; }
    public int PaletteId { get; set; }
    public int Type { get; set; }
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public bool IsArchived { get; set; }
}

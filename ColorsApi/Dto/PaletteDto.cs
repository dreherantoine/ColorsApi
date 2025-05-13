using ColorsApi.Models;
using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class PaletteDto
{
    [JsonRequired]
    public IReadOnlyCollection<ColorCode> Colors { get; set; }
}

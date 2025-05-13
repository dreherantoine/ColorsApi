using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class ColorCodeDto
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

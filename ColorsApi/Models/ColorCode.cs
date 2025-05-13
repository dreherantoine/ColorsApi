using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ColorsApi.Models;

public class ColorCode
{
    public int Type { get; set; }
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
}

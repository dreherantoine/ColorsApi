using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class LoginDto
{
    [JsonRequired]
    public string Email { get; set; }
    [JsonRequired]
    public string Password { get; set; }
}

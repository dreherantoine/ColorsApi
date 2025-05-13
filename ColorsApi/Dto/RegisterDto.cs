using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class RegisterDto
{
    [JsonRequired]
    public string Email { get; set; }
    [JsonRequired]
    public string Password { get; set; }
    [JsonRequired]
    public string PasswordConfirm { get; set; }
}

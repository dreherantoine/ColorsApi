using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class RefreshTokenDto
{
    [JsonRequired]
    public string Token { get; set; }
}

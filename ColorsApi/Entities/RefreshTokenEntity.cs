using Microsoft.AspNetCore.Identity;

namespace ColorsApi.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}

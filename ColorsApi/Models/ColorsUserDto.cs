namespace ColorsApi.Models;

public class ColorsUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}

public class ColorsUserEntity
{
    public Guid Id { get; set; }
    public string IdentityId { get; set; }
    public string Email { get; set; }
}

using System.Text.Json.Serialization;
using FluentValidation;

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

public class RegisterDtoValidator: AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(12)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}

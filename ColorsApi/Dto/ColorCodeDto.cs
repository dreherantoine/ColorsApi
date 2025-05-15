using System.Text.Json.Serialization;
using FluentValidation;

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

public class ColorCodeDtoValidator : AbstractValidator<ColorCodeDto>
{
    public ColorCodeDtoValidator()
    {
        RuleFor(color => color.Type)
            .NotNull()
            .WithMessage("Invalid color type provided");

        RuleFor(color => color.Red).Must(value => value >= 0 && value <= 255);
        RuleFor(color => color.Green).Must(value => value >= 0 && value <= 255);
        RuleFor(color => color.Blue).Must(value => value >= 0 && value <= 255);
    }
}

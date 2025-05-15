using ColorsApi.Models;
using ColorsApi.Dto;
using FluentValidation;
using System.Text.Json.Serialization;

namespace ColorsApi.Dto;

public class PaletteDto
{
    [JsonRequired]
    public IReadOnlyCollection<ColorCodeDto> Colors { get; set; }
}

public class PaletteDtoValidator: AbstractValidator<PaletteDto>
{
    public PaletteDtoValidator()
    {
        RuleForEach(palette => palette.Colors)
            .SetValidator(new ColorCodeDtoValidator());

        RuleFor(palette => palette.Colors)
            .Must(colors => colors.Count == 5)
            .WithMessage("ColorPalette should contain five colors");
    }
}

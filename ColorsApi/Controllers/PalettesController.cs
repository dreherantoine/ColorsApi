using Microsoft.AspNetCore.Mvc;

namespace ColorsApi.Controllers;

[ApiController]
[Route("api/palettes")]
public class PalettesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetColorPalettes()
    {
        var randomPalettes = new CollectionResponse<PaletteDto>
        {
            Items = new List<PaletteDto>
            {
                PaletteDto.RandomPalette(),
                PaletteDto.RandomPalette(),
            }
        };

        return Ok(randomPalettes);
    }
}

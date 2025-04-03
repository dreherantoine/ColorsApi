using ColorsApi.Database;
using ColorsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Controllers;

[ApiController]
[Route("api/palettes")]
public class PalettesController(ColorsDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetColorPalettes()
    {
        var palettes = await dbContext
            .Palettes
            .Include(p => p.Colors)
            .ToListAsync();

        var response = new CollectionResponse<Palette>
        {
            Items = palettes.Select(p => new Palette
            {
                Colors = p.Colors.Select(c => new ColorCode
                {
                    Type = c.Type,
                    Red = c.Red,
                    Green = c.Green,
                    Blue = c.Blue
                }).ToList()
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost("random")]
    public async Task<IActionResult> AddRandomPalette()
    {
        var randomPalette = Palette.RandomPalette();

        var paletteEntity = new PaletteEntity
        {
            Colors = randomPalette.Colors.Select(c => new ColorEntity
            {
                Type = c.Type,
                Red = c.Red,
                Green = c.Green,
                Blue = c.Blue
            }).ToList()
        };

        dbContext.Palettes.Add(paletteEntity);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetColorPalettes), new { id = paletteEntity.Id }, paletteEntity);
    }
}

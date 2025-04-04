using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
    public async Task<IActionResult> GetColorsPalettes()
    {
        List<PaletteEntity> palettesEntity = await dbContext
            .Palettes
            .Include(p => p.Colors)
            .ToListAsync();

        var palettes = new CollectionResponse<Palette>
        {
            Items = palettesEntity.Select(p => new Palette
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

        return Ok(palettes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetColorsPalette(int id)
    {
        List<ColorEntity> colorsEntity = await dbContext
            .Colors
            .Where(c => c.PaletteId == id)
            .ToListAsync();

        if (colorsEntity == null)
        {
            return NotFound();
        }

        var colors = colorsEntity.Select(c => new ColorCode
        {
            Type = c.Type,
            Red = c.Red,
            Green = c.Green,
            Blue = c.Blue
        }).ToList();

        return Ok(colors);
    }

    [HttpPost]
    public async Task<IActionResult> AddPalette([FromBody] Palette palette)
    {
        var paletteEntity = new PaletteEntity
        {
            Colors = palette.Colors.Select(c => new ColorEntity
            {
                Type = c.Type,
                Red = c.Red,
                Green = c.Green,
                Blue = c.Blue
            }).ToList()
        };

        dbContext.Palettes.Add(paletteEntity);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddPalette), new { paletteEntity.Id, palette.Colors });
    }

    [HttpPost("random")]
    public async Task<IActionResult> AddRandomPalette()
    {
        var palette = Palette.RandomPalette();
        var paletteEntity = new PaletteEntity
        {
            Colors = palette.Colors.Select(c => new ColorEntity
            {
                Type = c.Type,
                Red = c.Red,
                Green = c.Green,
                Blue = c.Blue
            }).ToList()
        };

        dbContext.Palettes.Add(paletteEntity);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddRandomPalette), new { paletteEntity.Id, palette.Colors });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePalette(int id)
    {
        var paletteEntity = await dbContext
            .Palettes
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (paletteEntity == null)
        {
            return NotFound();
        }

        paletteEntity.IsArchived = true;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}

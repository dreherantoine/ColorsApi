using System.ComponentModel.DataAnnotations;
using ColorsApi.Database;
using ColorsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Controllers;

[ApiController]
[Route("api/colors")]
public class ColorsController(ColorsDbContext dbContext) : ControllerBase
{
    [HttpPost("{id}")]
    public async Task<IActionResult> AddColor(int id, [FromBody] ColorCode color)
    {
        var paletteEntity = await dbContext
            .Palettes
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (paletteEntity == null)
        {
            return NotFound();
        }

        var colorEntity = new ColorEntity
        {
            PaletteId = id,
            Type = color.Type,
            Red = color.Red,
            Green = color.Green,
            Blue = color.Blue,
        };

        dbContext.Colors.Add(colorEntity);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddColor), new { colorEntity.Id, color });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColor(int id, [FromBody] ColorCode color)
    {
        var colorEntity = await dbContext
            .Colors
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (colorEntity == null)
        {
            return NotFound();
        }

        colorEntity.Type = color.Type;
        colorEntity.Red = color.Red;
        colorEntity.Green = color.Green;
        colorEntity.Blue = color.Blue;

        await dbContext.SaveChangesAsync();

        return Ok(color);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColor(int id)
    {
        var colorEntity = await dbContext
            .Colors
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (colorEntity == null)
        {
            return NotFound();
        }

        colorEntity.IsArchived = true;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}

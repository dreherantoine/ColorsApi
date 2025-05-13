using System.ComponentModel.DataAnnotations;
using ColorsApi.Database;
using ColorsApi.Dto;
using ColorsApi.Entities;
using ColorsApi.Models;
using ColorsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ColorsApi.Controllers;

[ApiController]
[Route("api/colors")]
public class ColorsController(ColorsDbContext dbContext, UserService userService) : ControllerBase
{
    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> AddColor(int id, [FromBody] ColorCodeDto colorDto)
    {
        var userId = await userService.GetUserIdAsync();
        if (userId.HasNoValue)
        {
            return Unauthorized();
        }

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
            Type = colorDto.Type,
            Red = colorDto.Red,
            Green = colorDto.Green,
            Blue = colorDto.Blue,
        };

        dbContext.Colors.Add(colorEntity);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(AddColor), new { colorEntity.Id, colorDto });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateColor(int id, [FromBody] ColorCodeDto colorDto)
    {
        var userId = await userService.GetUserIdAsync();
        if (userId.HasNoValue)
        {
            return Unauthorized();
        }

        var colorEntity = await dbContext
            .Colors
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (colorEntity == null)
        {
            return NotFound();
        }

        colorEntity.Type = colorDto.Type;
        colorEntity.Red = colorDto.Red;
        colorEntity.Green = colorDto.Green;
        colorEntity.Blue = colorDto.Blue;

        await dbContext.SaveChangesAsync();

        return Ok(new { colorEntity.Id, colorDto });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColor(int id)
    {
        var userId = await userService.GetUserIdAsync();
        if (userId.HasNoValue)
        {
            return Unauthorized();
        }

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

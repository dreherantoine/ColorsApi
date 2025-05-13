using System.Security.Claims;
using System.Text;
using ColorsApi.Database;
using ColorsApi.Models;
using ColorsApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ColorsApi.Controllers;

[ApiController]
[Route("api")]
public class RefreshTokenController(
    ColorsDbContext colorsDbContext,
    AppIdentityDbContext identityDbContext,
    UserManager<IdentityUser> userManager,
    IOptions<JwtAuthOptions> options) : ControllerBase
{
    private readonly JwtAuthOptions jwtAuthOptions = options.Value;
    private string CreateToken(string userId, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtAuthOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtAuthOptions.Issuer,
            Audience = jwtAuthOptions.Audience,
        };

        var handler = new JsonWebTokenHandler();
        string accessToken = handler.CreateToken(tokenDescriptor);

        return accessToken;
    }

    [HttpPost("access-token")]
    public async Task<IActionResult> Register([FromBody] ColorsUserDto colorUserDto)
    {
        var identityUser = new IdentityUser
        {
            Email = colorUserDto.Email,
            UserName = colorUserDto.Email,
        };

        IdentityResult createUserResult = await userManager.CreateAsync(identityUser, colorUserDto.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        var user = new ColorsUserEntity
        {
            IdentityId = identityUser.Id,
            Email = colorUserDto.Email,
        };

        colorsDbContext.Users.Add(user);
        await colorsDbContext.SaveChangesAsync();

        string accessTokens = CreateToken(identityUser.Id, identityUser.Email);

        await identityDbContext.SaveChangesAsync();

        return Ok(accessTokens);
    }
}

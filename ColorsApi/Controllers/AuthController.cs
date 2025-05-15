using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ColorsApi.Database;
using ColorsApi.Dto;
using ColorsApi.Entities;
using ColorsApi.Models;
using ColorsApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ColorsApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    ColorsDbContext colorsDbContext,
    AppIdentityDbContext identityDbContext,
    UserManager<IdentityUser> userManager,
    IOptions<JwtAuthOptions> options) : ControllerBase
{
    private readonly JwtAuthOptions jwtAuthOptions = options.Value;
    private AccessTokenDto CreateTokens(string userId, string email)
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
        string refreshToken = GenerateRefreshToken();

        return new AccessTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    [HttpPost("access-token")]
    public async Task<ActionResult<AccessTokenDto>> Register(
        [FromBody] RegisterDto registerDto,
        IValidator<RegisterDto> validator)
    {
        await validator.ValidateAndThrowAsync(registerDto);

        if (registerDto.Password != registerDto.PasswordConfirm)
        {
            return BadRequest("Passwords do not match");
        }

        var identityUser = new IdentityUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
        };

        IdentityResult createUserResult = await userManager.CreateAsync(identityUser, registerDto.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        var user = new ColorsUserEntity
        {
            IdentityId = identityUser.Id,
            Email = registerDto.Email,
        };

        colorsDbContext.Users.Add(user);
        await colorsDbContext.SaveChangesAsync();

        AccessTokenDto accessTokens = CreateTokens(identityUser.Id, identityUser.Email);
        var refreshToken = new RefreshTokenEntity
        {
            UserId = identityUser.Id,
            Token = accessTokens.RefreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(jwtAuthOptions.RefreshTokenExpirationDays),
        };
        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync();

        return Ok(accessTokens);
    }

    [HttpPut("access-token")]
    public async Task<ActionResult<AccessTokenDto>> Login(LoginDto loginDto)
    {
        var identityUser = await userManager.FindByEmailAsync(loginDto.Email);

        if (identityUser == null)
        {
            return NotFound("User not found");
        }

        AccessTokenDto accessTokens = CreateTokens(identityUser.Id, identityUser.Email!);

        RefreshTokenEntity? refreshToken = await identityDbContext.RefreshTokens
            .Include(token => token.User)
            .Where(token => token.ExpirationDate > DateTime.UtcNow)
            .FirstOrDefaultAsync(token => token.UserId == identityUser.Id);

        if (refreshToken == null)
        {
            return BadRequest("Refresh token expired");
        }
        
        refreshToken.Token = accessTokens.RefreshToken;

        await identityDbContext.SaveChangesAsync();

        return Ok(accessTokens);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AccessTokenDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        RefreshTokenEntity? refreshToken = await identityDbContext.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Token == refreshTokenDto.Token);

        if (refreshToken == null ||
            refreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        AccessTokenDto accessTokens = CreateTokens(refreshToken.UserId, refreshToken.User.Email!);

        refreshToken.Token = accessTokens.RefreshToken;
        refreshToken.ExpirationDate = DateTime.UtcNow.AddDays(jwtAuthOptions.RefreshTokenExpirationDays);

        await identityDbContext.SaveChangesAsync();

        return Ok(accessTokens);
    }
}

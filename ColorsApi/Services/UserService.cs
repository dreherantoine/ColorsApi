using ColorsApi.Database;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ColorsApi.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, ColorsDbContext dbContext)
{
    public async Task<Maybe<Guid>> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        string? identityId = httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (identityId is null)
        {
            return Maybe.None;
        }

        Guid? userId = await dbContext.Users
            .Where(u => u.IdentityId == identityId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return userId.HasValue ? Maybe.From(userId.Value) : Maybe.None;
    }
}

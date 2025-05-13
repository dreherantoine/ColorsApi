
using System.Threading.Tasks;
using ColorsApi.Configurations;
using ColorsApi.Database;
using ColorsApi.Models;
using ColorsApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ColorsApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.ConfigureTelemetry();
        builder.AddDatabase();
        builder.AddAuthenticationServices();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<UserService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            await app.ApplyMigrationsAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        await app.RunAsync();
    }
}

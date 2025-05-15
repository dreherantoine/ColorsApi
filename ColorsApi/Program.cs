
using System.Threading.Tasks;
using ColorsApi.Configurations;
using ColorsApi.Database;
using ColorsApi.Dto;
using ColorsApi.Models;
using ColorsApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ColorsApi;

public class Program
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
        builder.AddErrorHandling();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IValidator<ColorCodeDto>, ColorCodeDtoValidator>();
        builder.Services.AddScoped<IValidator<PaletteDto>, PaletteDtoValidator>();
        builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            await app.ApplyMigrationsAsync();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}

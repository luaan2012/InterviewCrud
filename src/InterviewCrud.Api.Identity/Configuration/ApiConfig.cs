using InterviewCrud.Api.Identity.Data;
using InterviewCrud.Api.Identity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace InterviewCrud.Api.Identity.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appSettings = new AppSettings();

        configuration.GetSection("AppJwtSettings").Bind(appSettings);

        services.Configure<AppSettings>(configuration.GetSection("AppJwtSettings"));

        services.AddEndpointsApiExplorer();

        services
            .AddJwksManager()
            .UseJwtValidation()
            .PersistKeysToDatabaseStore<ApplicationDbContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = appSettings.ValidadeIssuer,
                    ValidateAudience = appSettings.ValidateAudience,
                    ValidateLifetime = appSettings.ValidateLifetime,
                    ValidateIssuerSigningKey = appSettings.ValidateIssuerSigningKey,
                    ValidIssuer = appSettings.ValidIssuer,
                    ValidAudience = appSettings.ValidAudience
                };
            });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader();
            });
        });

        services.AddAuthorization();

        services.AddMemoryCache();

        services.AddHttpContextAccessor();
    }

    public static void UseApiConfiguration(this IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseJwksDiscovery();

        app.UseCors();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHttpsRedirection();
    }
}
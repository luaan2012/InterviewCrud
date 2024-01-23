using InterviewCrud.Api.Client.Data;
using InterviewCrud.Api.Client.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtExtensions;

namespace InterviewCrud.Api.Client.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        var appSettings = new AppSettings();

        configuration.GetSection("Identity").Bind(appSettings);

        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                
                jwt.RequireHttpsMetadata = true;
                jwt.SaveToken = true;
                jwt.SetJwksOptions(new JwkOptions(appSettings.IdentityURL));
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
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();
    }
}

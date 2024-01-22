using InterviewCrud.Api.Identity.Data;
using InterviewCrud.Api.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InterviewCrud.Api.Identity.Configuration;

public static class IdentityConfig
{

    public static void AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddIdentity<User, IdentityRole>()
            .AddErrorDescriber<IdentityMessageConfig>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddPasswordConfiguration();
    }
}
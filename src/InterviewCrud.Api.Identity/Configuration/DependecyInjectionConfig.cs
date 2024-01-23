using InterviewCrud.Api.Identity.Helper;
using InterviewCrud.Api.Identity.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace InterviewCrud.Api.Identity.Configuration;

public static class DependecyInjectionConfig
{
    public static void AddDependecyInjectionConfig(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, Emailsender>();
        services.AddScoped<IAuthServices, AuthServices>();
		services.AddScoped<IAspNetUser, AspNetUser>();
	}
}
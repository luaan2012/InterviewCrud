using InterviewCrud.Api.Client.Repository;
using InterviewCrud.Api.Client.Services;

namespace InterviewCrud.Api.Client.Configuration;

public static class DependecyInjectionConfig
{
    public static void AddDependecyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientService, ClientService>();
    }
}


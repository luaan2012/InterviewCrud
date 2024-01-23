using InterviewCrud.Api.Client.Configuration;
using InterviewCrud.Api.Client.Data;
using InterviewCrud.Api.Client.Endpoints;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddDependecyInjectionConfig();

builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.UseSwaggerConfig();

//if (builder.Environment.IsProduction())
//{
//    IdentityModelEventSource.ShowPII = true;

//    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
//    await db.Database.EnsureCreatedAsync();
//}

app.UseApiConfiguration(builder.Environment);

app.MapUserEndpoints();

app.Run();

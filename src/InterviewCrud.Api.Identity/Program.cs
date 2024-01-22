using InterviewCrud.Api.Identity.Configuration;
using InterviewCrud.Api.Identity.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddDependecyInjectionConfig();

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.UseSwaggerConfig(builder.Environment);

//if (app.Environment.IsDevelopment())
//{
//    IdentityModelEventSource.ShowPII = true;

//    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    await db.Database.EnsureCreatedAsync();
//}

app.UseApiConfiguration();

app.MapUserEndpoints();

app.Run();

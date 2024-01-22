using InterviewCrud.Api.Identity.Configuration;
using InterviewCrud.Api.Identity.EndPoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddDependecyInjectionConfig();

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.UseSwaggerConfig(builder.Environment);

app.UseApiConfiguration();
app.MapUserEndpoints();

var physicalPathConfig = builder.Configuration.GetValue<string>("StaticFiles:Mapping:PhysicalPath");
var virtualPath = builder.Configuration.GetValue<string>("StaticFiles:Mapping:VirtualPath");
var appBasePath = AppContext.BaseDirectory;
var physicalPath = Path.Combine(appBasePath, physicalPathConfig);

if (!Directory.Exists(physicalPath))
{
    Directory.CreateDirectory(physicalPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(physicalPath),
    RequestPath = new PathString(virtualPath)
});

app.Run();

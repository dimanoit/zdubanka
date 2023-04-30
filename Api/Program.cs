using System.Text.Json.Serialization;
using Api;
using Api.Middlewares;
using Application;
using Infrastructure;
using Serilog;
using ConfigureServices = Api.ConfigureServices;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

var host = builder.Host;
host.UseSerilog();

var services = builder.Services;

services.AddInfrastructureServices(builder.Configuration);
services.AddApplicationServices();
services.AddApiServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting()
    .UseCors(ConfigureServices.CorsPolicyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
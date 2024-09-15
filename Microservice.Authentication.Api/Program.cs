using Microservice.Authentication.Api.Endpoints;
using Microservice.Authentication.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.ConfigureExceptionHandling();
builder.Services.AddControllers();
builder.Services.ConfigureMediatr();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDI();
builder.Services.ConfigureSqlServer(builder.Configuration, environment);
builder.Services.ConfigureApiVersioning();

var app = builder.Build();

app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.ConfigureMiddleware();

Endpoints.ConfigureRoutes(app);

app.Run();
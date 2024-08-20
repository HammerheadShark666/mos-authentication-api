using Microservice.Authentication.Api.Endpoints;
using Microservice.Authentication.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureExceptionHandling();
builder.Services.AddControllers();
builder.Services.ConfigureMediatr();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.ConfigureSwagger(); 
builder.Services.ConfigureDI();
builder.Services.ConfigureDatabaseContext(builder.Configuration);   
builder.Services.ConfigureApiVersioning();

var app = builder.Build(); 
 
app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.ConfigureMiddleware();

Endpoints.ConfigureRoutes(app);

app.Run();
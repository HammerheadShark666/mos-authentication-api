using Microservice.Authentication.Api.Endpoints;
using Microservice.Authentication.Api.Extensions;
using Microservice.Authentication.Api.Middleware;

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
app.UseMiddleware<ExceptionHandlingMiddleware>();
 
Endpoints.ConfigureRoutes(app);

app.Run();
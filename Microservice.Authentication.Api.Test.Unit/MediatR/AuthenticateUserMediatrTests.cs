using FluentValidation;
using MediatR;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers;
using Microservice.Authentication.Api.Helpers.Interfaces;
using Microservice.Authentication.Api.MediatR.AuthenticateUser;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework.Internal;

namespace Microservice.AuthenticateUser.Api.Test.Unit;

[TestFixture]
public class AuthenticateUserMediatrTests
{
    private Mock<IUserRepository> userRepositoryMock = new();
    private ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;
    private Mock<IJwtHelper> jwtHelperMock = new();


    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<AuthenticateUserValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AuthenticateUserQueryHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<IUserRepository>(sp => userRepositoryMock.Object);
        services.AddScoped<IJwtHelper>(sp => jwtHelperMock.Object);
        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Login_successfully_return_jwt_token()
    {
        string username = "intergration-test-user@example.com";
        string password = "Password#1";

        User user = new()
        {
            Id = new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"),
            Email = username,
            PasswordHash = "$2a$11$K7TSYHDJaepUjxZPiE4dY.tuzpiL2JoEItsb3CVqwNkNELXIX2Ywy",
            Role = Enums.Role.User,
            Verified = DateTime.Parse("2023-08-18 15:21:38")
        };

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBdXRoZW50aWNhdGlvbiIsImp0aSI6IjFmMTE1ODM2LTA4N2UtNDAyNS1hNjFlLTIxNmRkNTVjZDAxMiIsImlhdCI6MTcyNDE2NTc0MCwiZXhwIjoxNzI0MTY5MzQwLCJpc3MiOiJKb2huTWlsbGVySXNzdWVyIiwiYXVkIjoibWljcm9zZXJ2aWNlb3JkZXJzeXN0ZW0iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjZjODRkMGEzLTBjMGMtNDM1Zi05YWUwLTRkZTA5MjQ3ZWUxNSJ9.QD4XbgdzCEVPZE1m6O-TYbUnIMvA2h-kxE2q5T_9OV0";

        userRepositoryMock
                .Setup(x => x.GetAsync(username))
                .Returns(Task.FromResult(user));

        jwtHelperMock
                .Setup(x => x.generateJwtToken(user))
                .Returns(jwtToken);

        var authenticateUserRequest = new AuthenticateUserRequest(username, password);
        var actualResult = await mediator.Send(authenticateUserRequest);

        Assert.That(actualResult.JwtToken, Is.EqualTo(jwtToken));
    }

    [Test]
    public void Login_fail_return_400_exception()
    {
        string username = "intergration-test-user@example.com";
        string password = "Password#1";

        userRepositoryMock.Setup(m => m.GetAsync(username)).ReturnsAsync((User)null);

        var authenticateUserRequest = new AuthenticateUserRequest(username, password);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(authenticateUserRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(1));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Invalid login"));
    }
}
using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers;
using Microservice.Authentication.Api.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Microservice.AuthenticateUser.Api.Test.Unit;

[TestFixture]
public class JwtHelperTests
{
    private IJwtHelper _jwtHelper;


    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .Build();

        _jwtHelper = new JwtHelper(configuration);
    }

    [Test]
    public void Generate_jwt_token_successfully_return_jwt_token()
    {
        User user = new()
        {
            Id = new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15")
        };

        var actualResult = _jwtHelper.GenerateJwtToken(user);

        Assert.That(actualResult, Is.Not.Null);

        var userId = ValidateToken(actualResult);

        Assert.That(userId, Is.EqualTo(user.Id));
    }

    public Guid? ValidateToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(EnvironmentVariables.JwtSymmetricSecurityKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

}
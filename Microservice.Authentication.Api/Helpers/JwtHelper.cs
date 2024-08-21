using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservice.Authentication.Api.Helpers;
public class JwtHelper(IConfiguration configuration) : IJwtHelper
{
    private IConfiguration _configuration { get; set; } = configuration;

    private const int ExpirationMinutes = 60;

    public string generateJwtToken(User user)
    {
        var nowUtc = DateTime.UtcNow;
        var expirationTimeUtc = GetExpirationTimeUtc(nowUtc);

        var token = new JwtSecurityToken(issuer: EnvironmentVariables.JwtIssuer,
                                         claims: GetClaims(user, nowUtc, expirationTimeUtc),
                                         expires: expirationTimeUtc,
                                         signingCredentials: GetSigningCredentials());

        return GetTokenString(token);
    }

    private static string GetTokenString(JwtSecurityToken token)
    {
        return (new JwtSecurityTokenHandler()).WriteToken(token);
    }

    private DateTime GetExpirationTimeUtc(DateTime nowUtc)
    {
        var expirationDuration = TimeSpan.FromMinutes(GetExpirationMinutes());
        return nowUtc.Add(expirationDuration);
    }

    private int GetExpirationMinutes()
    {
        var expirationMinutes = _configuration["JwtToken:ExpirationMinutes"];

        if (expirationMinutes is null)
        {
            return ExpirationMinutes;
        }
        else
        {
            return Int32.Parse(expirationMinutes);
        }
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtSymmetricSecurityKey));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims(User user, DateTime nowUtc, DateTime expirationUtc)
    {
        return new List<Claim>
                    {
                        new(JwtRegisteredClaimNames.Sub, "Authentication"),
                        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(nowUtc).ToString(), ClaimValueTypes.Integer64),
                        new(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expirationUtc).ToString(), ClaimValueTypes.Integer64),
                        new(JwtRegisteredClaimNames.Iss, EnvironmentVariables.JwtIssuer),
                        new(JwtRegisteredClaimNames.Aud, EnvironmentVariables.JwtAudience),
                        new(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
    }
}
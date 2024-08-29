using Microservice.Authentication.Api.Domain;

namespace Microservice.Authentication.Api.Helpers.Interfaces;

public interface IJwtHelper
{
    string GenerateJwtToken(User user);
}

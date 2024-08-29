using MediatR;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers.Exceptions;
using Microservice.Authentication.Api.Helpers.Interfaces;

namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;

public class AuthenticateUserQueryHandler(IUserRepository userRepository,
                                          IJwtHelper jwtHelper) : IRequestHandler<AuthenticateUserRequest, AuthenticateUserResponse>
{
    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(request.Username) ?? throw new NotFoundException("User not found.");
        return new AuthenticateUserResponse(jwtHelper.GenerateJwtToken(user));
    }
}
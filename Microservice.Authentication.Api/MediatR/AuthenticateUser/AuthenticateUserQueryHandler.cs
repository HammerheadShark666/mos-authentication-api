using MediatR;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers.Interfaces;
using static Microservice.Authentication.Api.Helpers.Enums;

namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;

public class AuthenticateUserQueryHandler(IUserRepository userRepository,
                                          IJwtHelper jwtHelper,
                                          IWebHostEnvironment env) : IRequestHandler<AuthenticateUserRequest, AuthenticateUserResponse>
{
    private const int ExpirationMinutes = 60;
    private IUserRepository _userRepository { get; set; } = userRepository;
    private IWebHostEnvironment _env { get; set; } = env;
    private IJwtHelper _jwtHelper { get; set; } = jwtHelper; 

    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {   
        User? user = await _userRepository.GetAsync(request.Username);
        return new AuthenticateUserResponse(_jwtHelper.generateJwtToken(user)); 
    }
}
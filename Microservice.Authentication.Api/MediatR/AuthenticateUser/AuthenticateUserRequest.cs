using MediatR;

namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;

public record AuthenticateUserRequest(string Username, string Password) : IRequest<AuthenticateUserResponse>;
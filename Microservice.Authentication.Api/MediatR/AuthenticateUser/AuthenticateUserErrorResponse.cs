namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;


public record AuthenticateUserErrorResponse(int status, string Message, IEnumerable<string> Errors);
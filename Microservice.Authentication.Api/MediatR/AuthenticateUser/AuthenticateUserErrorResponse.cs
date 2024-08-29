namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;

public record AuthenticateUserErrorResponse(int Status, string Message, IEnumerable<string> Errors);
using FluentValidation;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using BC = BCrypt.Net.BCrypt;

namespace Microservice.Authentication.Api.MediatR.AuthenticateUser;

public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserRequest>
{
    private readonly IUserRepository _userRepository;

    public AuthenticateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        RuleFor(authenticateUserRequest => authenticateUserRequest.Username)
               .NotEmpty().WithMessage("Email is required.")
               .Length(8, 150).WithMessage("Email length between 8 and 150.")
               .EmailAddress().WithMessage("Invalid Email.");

        RuleFor(authenticateUserRequest => authenticateUserRequest.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 50).WithMessage("Password length between 8 and 50.");

        RuleFor(authenticateUserRequest => authenticateUserRequest).MustAsync(async (authenticaterUserRequest, cancellation) =>
        {
            return await ValidLoginDetails(authenticaterUserRequest);
        }).WithMessage("Invalid login");
    }

    protected async Task<bool> ValidLoginDetails(AuthenticateUserRequest authenticateUserRequest)
    {
        User? user = await _userRepository.GetAsync(authenticateUserRequest.Username);

        if (user == null || !user.IsAuthenticated || !BC.Verify(authenticateUserRequest.Password, user.PasswordHash))
            return false;

        return true;
    }
}
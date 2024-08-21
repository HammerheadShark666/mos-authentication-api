using Microservice.Authentication.Api.Domain;

namespace Microservice.Authentication.Api.Data.Repository.Interfaces;

public interface IUserRepository
{  
    Task<User?> GetAsync(string email);
}

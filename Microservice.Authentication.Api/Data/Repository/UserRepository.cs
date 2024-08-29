using Microservice.Authentication.Api.Data.Context;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using Microservice.Authentication.Api.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Authentication.Api.Data.Repository;

public class UserRepository(IDbContextFactory<UserDbContext> dbContextFactory) : IUserRepository
{
    public async Task<User> GetAsync(string email)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var user = await db.Users.AsNoTracking()
                             .Where(a => a.Email.Equals(email))
                             .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found.");
        return user;
    }
}
using Microservice.Authentication.Api.Data.Contexts;
using Microservice.Authentication.Api.Data.Repository.Interfaces;
using Microservice.Authentication.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Authentication.Api.Data.Repository;

public class UserRepository(IDbContextFactory<UserDbContext> dbContextFactory) : IUserRepository
{
    public IDbContextFactory<UserDbContext> _dbContextFactory { get; set; } = dbContextFactory;
          
    public async Task<User?> GetAsync(string email)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Users.AsNoTracking()
                             .Where(a => a.Email.Equals(email))
                             .FirstOrDefaultAsync();
    } 
}
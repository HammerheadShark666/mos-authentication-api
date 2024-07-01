using Microsoft.EntityFrameworkCore;

namespace Microservice.Authentication.Api.Data.Contexts;

public class UserDbContext : DbContext
{ 
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) 
    {} 

    public DbSet<Domain.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<Domain.User>();
    }
}
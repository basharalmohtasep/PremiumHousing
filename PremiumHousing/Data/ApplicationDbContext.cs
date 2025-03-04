using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace PremiumHousing.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, ModelBuilder modelBuilder) : base(options)
    {
        _configuration = configuration;
        modelBuilder.Entity<User>().HasKey(x => new { x.Id, x.Username });

    }
    public DbSet<User> User {  get; set; }
}

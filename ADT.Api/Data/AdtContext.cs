using ADT.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ADT.Api.Data;

public class AdtContext : DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "AdtDb");
    }
}
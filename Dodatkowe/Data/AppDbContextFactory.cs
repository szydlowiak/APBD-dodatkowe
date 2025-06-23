using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dodatkowe.Data;
    
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=dodatkowe.db");

        return new AppDbContext(optionsBuilder.Options);
    }
} // https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
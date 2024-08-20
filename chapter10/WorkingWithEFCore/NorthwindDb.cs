using Microsoft.EntityFrameworkCore; // TO use DbContext and so on

namespace Northwind.EntityModels;

// This manage the interaction to the Northwind Database
public class NorthwindDb : DbContext 
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string databaseFile = "Northwind.db";
        string path = Path.Combine(Environment.CurrentDirectory, databaseFile);

        string connectionString = $" Data Source={path}";
        WriteLine(connectionString);
        optionsBuilder.UseSqlite(connectionString);
    }
}


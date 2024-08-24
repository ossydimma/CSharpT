using Microsoft.EntityFrameworkCore; // To use DbContext and so on
using Microsoft.EntityFrameworkCore.Diagnostics; // To use RelationalEventId

namespace Northwind.EntityModels;

// This manage the interaction to the Northwind Database
public class NorthwindDb : DbContext 
{
    //Two properties map to tables in database
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string databaseFile = "Northwind.db";
        string path = Path.Combine(Environment.CurrentDirectory, databaseFile);

        string connectionString = $" Data Source={path}";
        WriteLine(connectionString);
        optionsBuilder.UseSqlite(connectionString);

        optionsBuilder.LogTo(WriteLine, new[] {RelationalEventId.CommandExecuting}) // This is the Console method.

        #if DEBUG
         .EnableSensitiveDataLogging() // Include SQL parameters.
         .EnableDetailedErrors()
        #endif
        ;
        


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Example of using Fluent API instead of attributes
        // to limit the length of a category name to 15.
        modelBuilder.Entity<Category>()
            .Property(Category => Category.CategoryName)
            .IsRequired() // NOT NUll
            .HasMaxLength(15);

        // Some SQLite-specific configuration.
        if (Database.ProviderName?.Contains("Sqlite") ?? false )
        {
            // TO "fix" the lack of decimal support in SQLITE
            modelBuilder.Entity<Product>()
                .Property(product => product.Cost)
                .HasConversion<double>();
        }
    }
}


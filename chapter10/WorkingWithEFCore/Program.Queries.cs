using Microsoft.EntityFrameworkCore; // To use Include method
using Northwind.EntityModels; // To use Northwind, category, product
partial class Program
{
    private static void QueryingCategories()
    {
        using NorthwindDb db = new();

        SectionTitle("Categories and how many products they have");

        //Quarying to get all categories and their related products
        IQueryable<Category>? categories = db.categories?
            .Include(c => c.Products);

        if (categories is null || !categories.Any())
        {
            Fail("NO Category found");
            return;
        }

        // Execute query and enumerate results.
        foreach (Category c in categories)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }
    }

    private static void FilteredIncludes()
    {
        using NorthwindDb db = new();

        SectionTitle("Products with a minimum number of units in stock");

        string? input;
        int stock;

        do
        {
            Write("Enter a minimum for units in stock: ");
            input = ReadLine();
        } while (!int.TryParse(input, out stock));

        IQueryable<Category> categories = db.categories.Include(c => c.Products
            .Where(p => p.Stocks >= stock));

        if (categories is null || !categories.Any())
        {
            Fail("No category found");
            return;
        }

        Info($"ToQueryString: {categories.ToQueryString()}");

        foreach (Category c in categories)
        {
            WriteLine(
            "{0} has {1} products with a minimum {2} units in stock.",
            arg0: c.CategoryName, arg1: c.Products.Count, arg2: stock);

            foreach (Product p in c.Products)
            {
                WriteLine($" {p.ProductName} has {p.Stocks} units in stock.");
            }

        }
    }

    private static void QueryingProducts()
    {
        using NorthwindDb db = new();
        SectionTitle("Products that cost more than a price, highest at top");

        string? input;
        int price;

        do
        {
            Write("Enter a product Price: ");
            input = ReadLine();
        } while (!int.TryParse(input, out price));

        IQueryable<Product>? products = db.Products?
            .Where(p => p.Cost > price)
            .OrderByDescending(p =>  p.Cost);
        if (products is null || !products.Any())
        {
            Fail("No product found");
            return;
        }

        Info($"ToQueryString: {products.ToQueryString()}");


        foreach (Product p in products)
        {
            WriteLine("{0}: {1} cost {2:#$,##0.00} and has {3} in stock",
                p.ProductId, p.ProductName, p.Cost, p.Stocks);
        }
    }
}


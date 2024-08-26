using Microsoft.EntityFrameworkCore; //To use ExcuteUpdate, ExcuteDelete
using Microsoft.EntityFrameworkCore.ChangeTracking; //To use EntityEntry<T>
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Northwind.EntityModels; // To use Northwind, product

partial class Program
{
    private static void ListProducts(int[]? productIdsToHighlight = null)
    {
        using NorthwindDb db = new();
        if (db.Products == null || !db.Products.Any())
        {
            Fail("There are no Products.");
            return;
        }
        WriteLine("| {0,-3} | {1,-35} | {2,8} | {3,5} | {4} |",
        "Id", "Product Name", "Cost", "Stock", "Disc.");

        foreach(Product p in db.Products)
        {
            ConsoleColor previousColor = ForegroundColor;

            if (productIdsToHighlight is not null &&
            productIdsToHighlight.Contains(p.ProductId))
            {
                ForegroundColor = ConsoleColor.Green;
            }

            WriteLine("| {0:000} | {1,-35} | {2,8:$#,##0.00} | {3,5} | {4} |",
                p.ProductId, p.ProductName, p.Cost, p.Stocks, p.Discontinued);

            ForegroundColor = previousColor;
        }
    }
    // Adding Entities
    private static (int affected, int productId) AddProduct(int categoryId, string  productName, decimal? price, short? stock) 
    { 
        using NorthwindDb db = new();

        if (db.Products is null) return (0, 0);

        Product p = new() 
        { 
            CategoryId = categoryId,
            ProductName = productName,
            Cost = price,
            Stocks = stock
        };

        //set product as added in change tracking
        EntityEntry<Product> entity = db.Products.Add(p);
        WriteLine($"State: {entity.State}, ProductId: {p.ProductId}");

        //save tracked change to database
        int affected = db.SaveChanges();
        WriteLine($"State: {entity.State}, ProductId: {p.ProductId}");

        return (affected, p.ProductId);

    }

    // Updating entities

    private static (int affected, int productId) IncreaseProductPrice( string productNameStartsWith, decimal amount)
    {
        using NorthwindDb db = new();

        if (db.Products is null) return (0, 0);

        // Get the first product whose name starts with the parameter value.
        Product updateProduct = db.Products.First(
        p => p.ProductName.StartsWith(productNameStartsWith));

        updateProduct.Cost += amount;
        int affected = db.SaveChanges();

        return (affected, updateProduct.ProductId);
    }
    
    // deleting entities
    private static int DeleteProducts(string productStartsWith)
    {
        using NorthwindDb db = new();

        IQueryable<Product>? products = db.Products?
            .Where(p => p.ProductName.StartsWith(productStartsWith));

        if (products is null || !products.Any())
        {
            Fail("No products found to delete");
            return 0;
        }
        else
        {
            if (db.Products is null) return 0;

            db.Products.RemoveRange(products);
        }

        int affected = db.SaveChanges();

        return affected;

    }

    //Updating entity using ExcutedUpdate method
    private static (int affected, int[]? productId) IncreaseProductPriceBetter( string productNameStartsWith, decimal amount)
    {
        using NorthwindDb db = new();

        if (db.Products is null) return (0, null);

        IQueryable<Product> products = db.Products.Where(p => p.ProductName.StartsWith (productNameStartsWith));

        int affected = products.ExecuteUpdate(s => s.SetProperty(
            p => p.Cost, //using lamba to get product cost
            p => p.Cost + amount
            ));

        int[] productId = products.Select(p => p.ProductId).ToArray();

        return (affected, productId);
    }


}



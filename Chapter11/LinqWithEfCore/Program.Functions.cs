using Northwind.EntityModel; // To use NorthwindDb. category, product
using Microsoft.EntityFrameworkCore; // To use DbSet<I>

partial class Program 
{ 
    private static void FilterAndSort()
    {
        SectionTitle(" filter and sort");

        using NorthwindDb db = new();

        DbSet<Product> allProducts = db.Products;

        IQueryable<Product> filteredProducts = allProducts
            .Where(p => p.UnitPrice < 10M);

        IOrderedQueryable<Product> sortedAndFilteredProducts = filteredProducts
            .OrderByDescending(p => p.UnitPrice);

        var projectedProducts = sortedAndFilteredProducts
            .Select(product => new 
            { 
                product.ProductId,
                product.UnitPrice,
                product.ProductName

            });


        WriteLine("Products that cost less than $10");
        WriteLine(projectedProducts.ToQueryString());

        foreach (var p in projectedProducts)

        {
            WriteLine("{0}: {1} costs {2:$#,##0.00}",
                p.ProductId, p.ProductName, p.UnitPrice);
        }
        WriteLine();
    }

}


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

    private static void JoinCategoriesAndProducts()
    {
        SectionTitle("Join categories and products");

        using NorthwindDb db = new();

        var queryJoin = db.Categories.Join(
          inner: db.Products,
          outerKeySelector: category => category.CategoryId,
          innerKeySelector: product => product.CategoryId,
          resultSelector: (c, p) =>
          new { c.CategoryName, p.ProductName, p.ProductId })
            .OrderBy(cp => cp.CategoryName);

        foreach(var p in queryJoin)
        {
            WriteLine($"{p.ProductId}: {p.ProductName} in {p.CategoryName}");
        }
    }

    private static void GroupJoinCategoriesAndProducts()
    {

        SectionTitle("Group Join categories and products");

        using NorthwindDb db = new();

        var queryGroupJoin = db.Categories.AsEnumerable().GroupJoin(
            inner: db.Products,
            outerKeySelector: c => c.CategoryId,
            innerKeySelector: p => p.CategoryId,
            resultSelector: (c, matchingProducts) => new
            {
                c.CategoryName,
                products = matchingProducts.OrderBy(p => p.ProductName)
            });

        foreach(var c in queryGroupJoin)
        {
            WriteLine($"{c.CategoryName} has {c.products.Count()} Products");

            foreach(var p in c.products)
            {
                WriteLine(p.ProductName);
            }
        }

    }

    private static void ProductsLookup()
    {
        SectionTitle("Products Lookup");

        using NorthwindDb db = new();

        // Join all products to their category to return 77 matches.
        var productsQuery = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: c => c.CategoryId,
            innerKeySelector: p => p.CategoryId,
            resultSelector: (c, p) => new { c.CategoryName, products = p });


        ILookup<string, Product> productsLookup = productsQuery.ToLookup(
            keySelector: cp => cp.CategoryName,
            elementSelector: cp => cp.products
            );
        foreach(IGrouping<string, Product> group in productsLookup)
        {
            // Key is Beverages, Condiments, and so on.
            WriteLine($"{group.Key} has {group.Count()} products");

            foreach(Product product in group)
            {
                WriteLine(product.ProductName);
            }
        }

        // We can look up the products by a category name.
        Write("Enter a category name: ");
        string categoryName = ReadLine()!;
        WriteLine();
        WriteLine($" Products in {categoryName}");

        IEnumerable<Product> productsInCategory = productsLookup[categoryName];

        foreach(Product product in productsInCategory)
        {
            WriteLine(product.ProductName);
        }

    }

    private static void AggregateProducts()
    {
        SectionTitle(" Aggregate products ");

        using NorthwindDb db = new();

        // Try to get an efficient count from EF Core DbSet<T>.
        if (db.Products.TryGetNonEnumeratedCount(out int countDbSet))
        {
            WriteLine($"{"Product count for Dbset:", -25} {countDbSet,10}");
        }
        else
        {
            WriteLine("Product Dbset does'nt have a count property");
        }

        // Try to get an efficient count from a List<T>.
        List<Product> products = db.Products.ToList();
         
        if(products.TryGetNonEnumeratedCount(out int countList))
        {
            WriteLine($"{"product count for list:",-25} {countList,10}");
        }
        else
        {
            WriteLine("product list does not have a count property");
        }

        WriteLine($"{"Product count:",-25} {db.Products.Count(),10}");

        WriteLine($"{"Discontinued product count:",-27} {db.Products.Count(p => p.Discontinued),8}");

        WriteLine($"{"Highest product price:",-25} {db.Products.Max(p => p.UnitPrice),10:$#,##0.00}");

        WriteLine($"{"Sum of units in stock:",-25} {db.Products.Sum(p => p.UnitsInStock),10:N0}");

        WriteLine($"{"Sum of units on order:",-25} {db.Products.Sum(p => p.UnitsOnOrder),10:N0}");

        WriteLine($"{"Average units price:",-25} {db.Products.Average(p => p.UnitPrice),10:$#,##0.00}");

        WriteLine($"{"Value of units in stock:",-25} {db.Products.Sum(p => p.UnitPrice * p.UnitsInStock),10:$#,##0.00}");

    }

}


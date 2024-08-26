using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels; // To use Northwind, product
using System.Xml.Serialization; // To use XmlSerlizer
using fastJson = System.Text.Json.JsonSerializer; // to use Json serialzer
using System.Text.Json; // To use JsonSerializer.

using NorthwindDb db = new();

//if (db.categories is null )
//{
//    WriteLine("Product is null");
//    return;
//}

IQueryable<Category>? categories = db.categories
    .Include(c => c.Products);

if (categories is null || !categories.Any()) 
{ 
    WriteLine("No category found");
    return;
}
WriteLine($" categories: {categories}");

// Serialize the data using JSON serialization
string jsonData = Serialize(categories, "json");
WriteLine($"JSON serialization size: {jsonData.Length} bytes");

static string Serialize(IQueryable<Category> cate, string formate)
{
    string serializedData = " ";

    if (formate == "xml")
    {
        XmlSerializer xs = new(type: cate.GetType());

        string path = Path.Combine(Environment.CurrentDirectory, "products.xml");

        using (FileStream xmlstream = File.Create(path))
        {
            xs.Serialize(xmlstream, cate);
            serializedData = xmlstream.ToString()!;
        };
    }
    else if (formate == "json")
    {
        JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            MaxDepth = 128
        };

        string jsonPath = Path.Combine(Environment.CurrentDirectory, "products.json");
        using (FileStream  jsonstream = File.Create(jsonPath))
        {
            Newtonsoft.Json.JsonSerializer jss = new();
            fastJson.Serialize(jsonstream, cate, options);
            serializedData= jsonstream.ToString()!;
        }

    }

     return serializedData;


}

//foreach(Category c in categories)
//{
//    WriteLine($"{c.CategoryName} has {c.Products.Count} products.");

//}
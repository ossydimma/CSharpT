using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels; // To use Northwind, product
//using System.Text.Json; // To use JsonSerializer.
using Newtonsoft.Json;// To use JsonSerializer.

using NorthwindDb db = new();



IQueryable<Category>? categories = db.categories
    .Include(c => c.Products);

if (categories is null || !categories.Any()) 
{ 
    WriteLine("No category found");
    return;
}


//THIS IS A DIFFERENT ROUTE TO USE IN PLACE OF [JSONIGNORE] ATTRIBUTE IT GLOBALLY IGNORE CIRCULAR REFERENCE 
// WHILE [JSONIGNORE] ATTRIBUTE  allows you to selectively ignore specific properties during serialization.
//var settings = new JsonSerializerSettings
//{
//    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//};



// Serialize the data using Newton JSON serialization
string jsonData = JsonConvert.SerializeObject(categories); // used JsonConvert 
WriteLine($"JSON serialization size: {jsonData.Length} bytes");

////Serialize the data using System.Text.Json JSON serialization 
//string data = JsonSerializer.Serialize(categories);
//WriteLine($"Fast JSON serialization size: {data.Length} bytes");



using Microsoft.AspNetCore.Mvc.RazorPages; // To use PageModel.
using Microsoft.EntityFrameworkCore; // To use Include method.
using Northwind.EntityModels; // To use Customer.

namespace Northwind.Web.Pages;

public class CustomerOrdersModel : PageModel
{
    public Customer? Customer;
    public IList<Order>? Orders {  get; set; }

    private NorthwindContext _db;

    public CustomerOrdersModel(NorthwindContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Customers and their orders";
        string? id = HttpContext.Request.Query["id"];

        Customer = _db.Customers
          .FirstOrDefault(c => c.CustomerId == id);

        if (Customer is null)
        {
            Console.WriteLine("no orders found");
            return;
        }

        Orders = _db.Orders
           .Where(o => o.CustomerId == Customer.CustomerId)
           .ToList(); 

        foreach (Order o in Customer.Orders)
        {
            Console.WriteLine($"{o.OrderId} was ordered on {o.OrderDate}");
        }
    }
}

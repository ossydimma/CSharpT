using Microsoft.AspNetCore.Mvc.RazorPages; // To use PageModel
using Northwind.EntityModels; // To use Customers


namespace Northwind.Web.Pages
{
    public class CustomersModel : PageModel
    {
        private NorthwindContext _db;

        public CustomersModel (NorthwindContext db)
        {
            _db = db;
        }

        public ILookup< string?, Customer >?  CustomersByCountry ;
        public void OnGet()
        {
            ViewData["Title"] = "Northwind B2B - Customers by Country";

            CustomersByCountry = _db.Customers
                .ToLookup(c => c.Country);
        }
    }
}
